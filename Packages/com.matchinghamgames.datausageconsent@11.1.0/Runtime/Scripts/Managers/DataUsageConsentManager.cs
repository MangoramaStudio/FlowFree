using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Bidtellect.Tcf.Models;
using Bidtellect.Tcf.Serialization;
using MatchinghamGames.Handyman.Managers;
using MatchinghamGames.Handyman;
using MatchinghamGames.Handyman.Coroutines;
using MatchinghamGames.Handyman.Utilities;
using MatchinghamGames.PrivacyPolicyPopup;
using MatchinghamGames.StashModule;
using MatchinghamGames.Utilities;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Scripting;
using Utilities;
using Object = UnityEngine.Object;

#if LOG4NET
using log4net;
#endif

#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif

[assembly: AlwaysLinkAssembly]

namespace MatchinghamGames.DataUsageConsent
{
    public enum UserAdTrackingState
    {
        Unknown = 0,
        NotAllowed,
        Allowed
    }

    public enum UserResponse
    {
        Unknown = 0,
        Yes,
        No
    }

    public class DataUsageConsentManager : Module<DataUsageConsentManager, DataUsageConsentConfig, DataUsageConsentDebugger>
    {
        #region Injection

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Inject()
        {
            Instance.InitializeDebugger();
            if (!Config.AutoInitialize) return;
            Instance.Logger.Info("Auto Initializing");
            Instance.Initialize();
        }

        #endregion

        /// <summary>
        /// Service of the module
        /// </summary>
        public static IDataUsageConsentService<IDataUsageConsentServiceConfig> Service { get; private set; }
        
        [field: AddToLoggerConfig(nameof(DataUsageConsentManager), LogLevel.Info, LogLevel.Info, LogLevel.Error)]
        public override ILog Logger { get; } = LogManager.GetLogger(nameof(DataUsageConsentManager));
        
        private static Stash _stash;
        internal static Stash Stash => _stash ??= Stash.PlayerPrefs(nameof(DataUsageConsentManager), null, false);

        private static readonly WebClient _webClient = new WebClient();
        
        internal static bool GeoLocationFetchingFailed;

        private static Area _currentArea;
        
        private static Action<InitializationResult> _initResult;

        public static Area CurrentArea => Service?.CurrentArea ?? _currentArea;
        public static bool IsATTRequired { get; private set; }
        
        private static bool PreATTPopupEnabled
        {
            get
            {
#if UNITY_IOS
                return Config.PreATTPopupEnabled;
#endif
                return false;
            }
        }
        
#if UNITY_IOS
        public static UserAdTrackingState UserAdTracking
        {
            get
            {
#if UNITY_EDITOR
                return (UserAdTrackingState)PlayerPrefs.GetInt(nameof(UserAdTracking),
                    (int)UserAdTrackingState.Unknown);
#endif
                
                var trackingStatus = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();

                switch (trackingStatus)
                {
                    case ATTrackingStatusBinding.AuthorizationTrackingStatus.DENIED:
                    case ATTrackingStatusBinding.AuthorizationTrackingStatus.RESTRICTED:
                        return UserAdTrackingState.NotAllowed;
                    
                    case ATTrackingStatusBinding.AuthorizationTrackingStatus.AUTHORIZED:
                        return UserAdTrackingState.Allowed;
                    
                    default:
                        return UserAdTrackingState.Unknown;
                }
            }
        }
#else
        public static UserAdTrackingState UserAdTracking { get; } = UserAdTrackingState.Unknown;

#endif
        
        [Obsolete("Use GetConsent method instead. This will be removed in future versions.")]
        public static UserResponse UserConsent
        {
            get => (UserResponse)PlayerPrefs.GetInt("userConsent", 0);
            internal set => PlayerPrefs.SetInt("userConsent", (int)value);
        }

        public static bool IsConsentRequiredInThisLocationFromCmp => Service?.IsConsentRequiredInThisLocation ?? false;
        public static bool ShouldCollectConsentFromCmp => Service?.ShouldCollectConsent ?? false;
        
        public static string CountryCode => Service?.CountryCode;
        public static string RegionCode => Service?.RegionCode;

        internal static IDictionary<string, bool> ConsentMap
        {
            get => Stash.Get(nameof(ConsentMap), new Dictionary<string, bool>());
            set => Stash.Set(nameof(ConsentMap), value);
        } 

        private static string TcString
        {
            get => Stash.Get(nameof(TcString), string.Empty);
            set => Stash.Set(nameof(TcString), value);
        }

        public static TcString TcfMap { get; private set; }
        
#if UNITY_IOS
        public static event Action PreAttPopupClosed;
#endif

        public static event Action ConsentUpdated;
        
        public static event Action BasicConsentFormClosed
        {
            add
            {
                if (Service != null) Service.BasicConsentFormClosed += value;
            }
            remove
            {
                if (Service != null) Service.BasicConsentFormClosed -= value;
            }
        }
        
        public static event Action AdvancedConsentFormClosed
        {
            add
            {
                if (Service != null) Service.AdvancedConsentFormClosed += value;
            }
            remove
            {
                if (Service != null) Service.AdvancedConsentFormClosed -= value;
            }
        }
        
        /// <summary>
        /// Service registration
        /// </summary>
        /// <param name="service">service</param>
        public static void RegisterService(IDataUsageConsentService<IDataUsageConsentServiceConfig> service)
        {
            Service ??= service;
            
            Instance.Logger.Debug($"Registering Service: {Service.GetType().Name}");
        }
        
        protected override void Initialize(Action<InitializationResult> setResult)
        {
#if UNITY_EDITOR && UNITY_IOS
            IsATTRequired = UserAdTracking == UserAdTrackingState.Unknown;
#elif UNITY_IOS && !UNITY_EDITOR
            var attExists = new Version(UnityEngine.iOS.Device.systemVersion) >= new Version("14.5");
            if (attExists) IsATTRequired = UserAdTracking == UserAdTrackingState.Unknown;
#endif
            
            _initResult = setResult;
            
            Localization.WorldWide.Instance.Initialize(); //todo: worldwide should handle initialization itself
            
            if (!PrivacyPolicy.Instance.Ready)
            {
                Logger.Warn($"{nameof(PrivacyPolicy)} is required for {nameof(DataUsageConsentManager)}. Will continue after {nameof(PrivacyPolicy)} initialization");
            }

            PrivacyPolicy.Instance.WhenReady(_ =>
            {
                ContinueInitialization();
            });
            
            return;

            void ContinueInitialization()
            {
                Instance.Logger.Info($"User Response: {UserConsent}");

                InitializeService();
            }
        }
        
        /// <summary>
        /// Initialize current service
        /// </summary>
        private static void InitializeService()
        {
            if (Service == null)
            {
                Instance.Logger.Warn("No service registered. Continuing with default consent flow...");
                DefaultConsentFlow();
                return;
            }
            
            Instance.Logger.Debug($"Initializing Service: {Service.GetType().Name}");
            
            Service.ConsentUpdated += ServiceOnConsentUpdated;
            
            Service?.Initialize();
            Service?.WhenReady(OnServiceReady);
        }

        /// <summary>
        /// Invokes when the service initialization is finished
        /// </summary>
        /// <param name="result">Initialization result</param>
        private static void OnServiceReady(InitializationResult result)
        {
            if (result == InitializationResult.FailedToInitialize)
            {
                Instance.Logger.Warn("Service failed to initialize. Continuing with default consent flow...");
                ParseTcString();
                DefaultConsentFlow();
            }
            else if (result == InitializationResult.Initialized)
            {
                // when service is initialized, continue with service consent flow
                
                Instance.Logger.Debug($"Consent required from CMP: {ShouldCollectConsentFromCmp}");
                Instance.Logger.Debug($"Consent collected from CMP: {IsConsentRequiredInThisLocationFromCmp}");
                
                if (ShouldCollectConsentFromCmp)
                {
                    Instance.Logger.Debug("Consent required from CMP. Starting CMP Consent Flow...");
                    CmpConsentFlow();
                    return;
                }
                
                if (UserConsent == UserResponse.Unknown)
                {
                    Instance.Logger.Debug("Consent not required from CMP and UserConsent is unknown. Starting Default Consent Flow...");
                    DefaultConsentFlow();
                    return;
                }
                
                Instance.Logger.Debug($"UserConsent is already given: {UserConsent.ToString()}. Finishing initialization...");
                FinishInitialization();
            }
        }

        private static void CmpConsentFlow()
        {
            Instance.Logger.Debug("CMP Consent Flow started");
            Instance.Logger.Debug("Showing Basic Consent Form...");
            
            Service.BasicConsentFormClosed += OnBasicConsentFormClosed;
            ShowBasicConsentForm();
            return;

            void OnBasicConsentFormClosed()
            {
                Service.BasicConsentFormClosed -= OnBasicConsentFormClosed;
                
                Instance.Logger.Debug("Basic Consent Form Closed");
                
                if (UserConsent == UserResponse.Unknown)
                {
                    SetUserConsent(UserResponse.Yes);
                    
                    if (!IsATTRequired)
                    {
                        Instance.Logger.Debug("ATT is not required. Finishing initialization...");
                        FinishInitialization();
                        return;
                    }
                    
                    Instance.Logger.Debug("Requesting ATT...");
                    HandleRequestAtt(FinishInitialization);
                    return;
                }
                
                Instance.Logger.Debug($"Before CMP, UserConsent is already given: {UserConsent.ToString()}. Finishing initialization...");
                FinishInitialization();
            }
        }

        private static void DefaultConsentFlow()
        {
            Instance.Logger.Debug("Default Consent Flow started");

            if (!(Service?.CurrentArea.HasValue ?? false))
            {
                Instance.Logger.Debug("Current Area is not determined yet. Fetching GDPR status...");
                TimeoutManager.Instance.CreateContext(nameof(DataUsageConsentManager), 7);
                FetchGeoLocation().Start();
            }
            
            TimeoutManager.Instance.CreateContext(nameof(DataUsageConsentManager), 7);
            FetchGeoLocation().Start();
            
            if (UserConsent == UserResponse.Unknown)
            {
                if (!IsATTRequired)
                {
                    Instance.Logger.Debug("ATT is not required. Opening Default Popup...");
                    OpenDefaultPopup(FinishInitialization);
                    return;
                }
                
                Instance.Logger.Debug("Requesting ATT...");
                HandleRequestAtt(() =>
                {
                    if (PreATTPopupEnabled)
                    {
                        Instance.Logger.Debug("Pre ATT Popup was enabled. No need to open default popup. Finishing initialization...");
                        FinishInitialization();
                        return;
                    }
                    
                    Instance.Logger.Debug("Pre ATT Popup was not enabled. Opening Default Popup...");
                    OpenDefaultPopup(FinishInitialization);
                });
            }
            else
            {
                Instance.Logger.Debug($"UserConsent is already given: {UserConsent.ToString()}. Finishing initialization...");
                FinishInitialization();
            }
        }

        private static void HandleRequestAtt(Action callback)
        {
#if UNITY_IOS
            var preAttPopupOpened = false;
            
            if (!IsATTRequired)
            {
                Instance.Logger.Debug("ATT is not required. Finishing ATT response...");
                FinalizeAttResponse();
                return;
            }

            if (PreATTPopupEnabled && !IsConsentRequiredInThisLocationFromCmp && !ShouldCollectConsentFromCmp)
            {
                Instance.Logger.Debug("Pre ATT Popup is enabled and consent is not collected and not required from CMP. Opening Pre ATT Popup...");
                preAttPopupOpened = true;
                PreAttPopupClosed += RequestAtt;
                OpenPreAttPopup();
                return;
            }

            RequestAtt();
            return;

            void RequestAtt()
            {
                if (preAttPopupOpened)
                {
                    PreAttPopupClosed -= FinalizeAttResponse;
                    SetUserConsent(UserResponse.Yes);
                }
                
                Instance.Logger.Debug($"Requesting ATT{(preAttPopupOpened ? " after Pre-ATT popup" : "")}...");

                // simulating ATT request on editor
#if UNITY_EDITOR
                // when i tried to open the popup directly, it couldn't be opened, so i'm moving it to the main thread
                UnityMainThreadDispatcher.Instance.Enqueue(() =>
                {
                    if (UnityEditor.EditorUtility.DisplayDialog("RequestAuthorizationTracking",
                            $"Allow \"{Application.productName}\" to track your activity across other companies' apps and websites?\n{Config.UsageDescription}",
                            "Ask App Not to Track", "Allow"))
                    {
                        PlayerPrefs.SetInt(nameof(UserAdTracking), (int)UserAdTrackingState.NotAllowed);
                        Instance.Logger.Debug("ATT Request completed with status: NotAllowed");
                        FinalizeAttResponse();
                    }
                    else
                    {
                        PlayerPrefs.SetInt(nameof(UserAdTracking), (int)UserAdTrackingState.Allowed);
                        Instance.Logger.Debug("ATT Request completed with status: Allowed");
                        FinalizeAttResponse();
                    }
                });
                return;
#endif
                
                ATTrackingStatusBinding.RequestAuthorizationTracking(status =>
                {
                    Instance.Logger.Debug($"ATT Request completed with status: {((ATTrackingStatusBinding.AuthorizationTrackingStatus)status).ToString()}");
                    FinalizeAttResponse();
                });
            }
#else
            FinalizeAttResponse();
#endif
            
            void FinalizeAttResponse()
            {
                callback?.Invoke();
            }
        }
        
#if UNITY_IOS
        private static void OpenPreAttPopup()
        {
            // this might cause a delay on text fetching but it's okay for now
            var localizedTexts = new GameObject(nameof(DataConsentLocalizedTexts)).AddComponent<DataConsentLocalizedTexts>(); 
            // todo: this should be handled better. e.g. separating native popup apis from here
            var p1 = localizedTexts.p1Text;
            var ppText = localizedTexts.ppText;
            var tcText = localizedTexts.tcText;
            var pressOkText = $"\n{localizedTexts.pressOkText}";
            var okText = localizedTexts.okText;
            
            p1 = p1.Replace("{0}",Application.productName);
            
            var preAttDialog = new DUCMNativeDialog(p1, ppText, tcText, PrivacyPolicy.PrivacyPolicyURL,
                PrivacyPolicy.TermsAndConditionsURL, pressOkText, okText);
            
            Instance.Logger.Debug("Opening Pre ATT popup");
            
            // simulating Pre ATT popup on editor
#if UNITY_EDITOR
            // when i tried to open the popup directly, it couldn't be opened, so i'm moving it to the main thread
            UnityMainThreadDispatcher.Instance.Enqueue(() =>
            {
                p1 = p1.Replace("{1}", ppText);
                p1 = p1.Replace("{2}", tcText);

                if (UnityEditor.EditorUtility.DisplayDialog("Pre-ATT Popup",
                        $"{p1}{pressOkText}",
                        okText))
                {
                    InvokePreAttPopupClosed();
                }
                else
                {
                    InvokePreAttPopupClosed();
                }
            });
            return;

#endif
            preAttDialog.AlertDialog();
        }
        
        internal static void InvokePreAttPopupClosed()
        {
            Instance.Logger.Debug("Pre ATT Popup closed");
            PreAttPopupClosed?.Invoke();
        }
#endif

        private static void OpenDefaultPopup(Action onDismissed)
        {
            Instance.Logger.Debug("Opening Default Popup...");
            
            // i got a weird exception from textmeshpro when i tried to open the popup directly, so i'm moving it to the main thread
            UnityMainThreadDispatcher.Instance.Enqueue(() =>
            {
                var prompt = Object.Instantiate(Config.DefaultPopupPrefab).GetComponent<ConsentPromptViewBase>();
                prompt.Initialize();
                prompt.OnPromptDismissed = () =>
                {
                    // popup sets consent to yes
                    Instance.Logger.Info($"Dismissed at prompt instantiated at {nameof(OpenDefaultPopup)}");
                    onDismissed?.Invoke();
                    Object.Destroy(prompt.gameObject);
                };    
            });   
        }

        private static void FinishInitialization()
        {
            _initResult?.Invoke(GeoLocationFetchingFailed
                ? InitializationResult.FailedToInitialize
                : InitializationResult.Initialized);
        }

        private static IEnumerator FetchGeoLocation() //todo: refactor this
        {
            Instance.Logger.Debug($"Checking connection...");

            // If no internet, ready with fail to initialize
            if (InternetConnectionManager.NetworkReachability == NetworkReachability.NotReachable)
            {
                EndFetchGdprRoutineWithFailure("No internet");
                yield break;
            }

            Instance.Logger.Debug($"Connected to network!");
            Instance.Logger.Debug($"Request geolocation check for EEA");
            string uri = "https://adservice.google.com/getconfig/pubvendors";

            switch (Config.GeoLocationDetectionMethod)
            {
                case DataUsageConsentConfig.GeoLocationDetectionURL.Google:
                    uri = "https://adservice.google.com/getconfig/pubvendors";
                    break;
                case DataUsageConsentConfig.GeoLocationDetectionURL.Unity:
                    uri = "https://pls.prd.mz.internal.unity3d.com/api/v1/user-lookup";
                    break;
            }

            try
            {
                _webClient.Encoding = Encoding.UTF8;
                _webClient.DownloadStringCompleted += OnDownloadStringCompleted;
                _webClient.DownloadStringAsync(new Uri(uri));
                TimeoutManager.Instance.StartListeningForTimeout(nameof(DataUsageConsentManager), OnTimeout);
            }
            catch (Exception ex)
            {
                EndFetchGdprRoutineWithFailure($"Request error - {ex.Message}");
                yield break;
            }
        }

        private static void OnTimeout()
        {
            TimeoutManager.Instance.StopListeningForTimeout(nameof(DataUsageConsentManager), OnTimeout);
            _webClient.DownloadStringCompleted -= OnDownloadStringCompleted;
            EndFetchGdprRoutineWithFailure("request time out");
        }

        private static void EndFetchGdprRoutineWithFailure(string reason)
        {
            Instance.Logger.Error($"GDPR fetching failed. Reason: {reason}");
            _currentArea = Area.Safe;
            GeoLocationFetchingFailed = true;
        }

        private static void OnDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            TimeoutManager.Instance.StopListeningForTimeout(nameof(DataUsageConsentManager), OnTimeout);
            if (sender is WebClient client)
                client.DownloadStringCompleted -= OnDownloadStringCompleted;

            if (e.Error == null)
            {
                try
                {
                    switch (Config.GeoLocationDetectionMethod)
                    {
                        case DataUsageConsentConfig.GeoLocationDetectionURL.Google:
                            var googleInfo = JsonUtility.FromJson<EEAInfo>(e.Result);

                            Instance.Logger.Info($"Request Finished ({e.Result})");
                            if (googleInfo.is_request_in_eea_or_unknown)
                            {
                                Instance.Logger.Info($"Current Area is EEA");
                                _currentArea = Area.EuropeEconomicArea;
                            }
                            else
                            {
                                Instance.Logger.Info($"Current Area is Safe");
                                _currentArea = Area.Safe;
                            }

                            break;
                        case DataUsageConsentConfig.GeoLocationDetectionURL.Unity:
                            var unityInfo = JsonUtility.FromJson<UnityGDPRInfo>(e.Result);

                            Instance.Logger.Info($"Request Finished ({e.Result})");
                            if (unityInfo.identifier.ToUpperInvariant().Equals("GDPR"))
                            {
                                Instance.Logger.Info($"Current Area is EEA");
                                _currentArea = Area.EuropeEconomicArea;
                            }
                            else if (unityInfo.identifier.ToUpperInvariant().Equals("CCPA"))
                            {
                                Instance.Logger.Info($"Current Area is California");
                                _currentArea = Area.California;
                            }
                            else
                            {
                                Instance.Logger.Info($"Current Area is Safe");
                                _currentArea = Area.Safe;
                            }

                            break;
                    }
                }
                catch (Exception ex)
                {
                    EndFetchGdprRoutineWithFailure(ex.Message);
                    return;
                }
            }
            else
            {
                EndFetchGdprRoutineWithFailure($"Request error - {e.Error.Message} with {Config.GeoLocationDetectionMethod}");
                return;
            }

            GeoLocationFetchingFailed = false;
        }

        private static UnityWebRequestAsyncOperation FetchRequestFromUrl(string url)
        {
            if (string.IsNullOrEmpty(url)) return null;
            if (string.IsNullOrWhiteSpace(url)) return null;

            var webRequest = UnityWebRequest.Get(url);
            var asyncOp = webRequest.SendWebRequest();

            return asyncOp;
        }
        
        public static void SetUserConsent(UserResponse response)
        {
            UserConsent = response;

            Instance.Logger.Info($"Setting user consent as {response}");
        }

        private static void ParseTcString()
        {
            if (string.IsNullOrEmpty(TcString))
            {
                Instance.Logger.Error("TCString is null or empty");
                return;
            }
            
            var parser = new TcStringParser(null);
            TcfMap = parser.Parse(TcString);
            
            Instance.Logger.Info(JsonConvert.SerializeObject(TcfMap, Formatting.Indented));
        }
        
        private static void ServiceOnConsentUpdated()
        {
            TcString = Service?.TCString;
            ConsentMap = Service?.ConsentMap;

            ParseTcString();

            Instance.Logger.Info($"Consent updated. TCString: {TcString}");
            
            ConsentUpdated?.Invoke();
        }

        /// <summary>
        /// Get consent for given non-TCF vendor's id
        /// </summary>
        /// <param name="templateID">non-TCF vendor's id</param>
        /// <returns>is consent given or not</returns>
        public static UserResponse GetConsent(string templateID)
        {
            if (!IsConsentRequiredInThisLocationFromCmp)
            {
                Instance.Logger.Debug($"Consent not required in this location from CMP. Returning Yes for {templateID}");
                return UserResponse.Yes;
            }

            if (Service.ConsentMap == null)
            {
                Instance.Logger.Warn("ConsentMap is null. Returning Unknown");
                return UserResponse.Unknown;
            }

            if (Service.ConsentMap.TryGetValue(templateID, out var consent))
            {
                Instance.Logger.Debug($"Consent for {templateID} is {consent}");
                return consent ? UserResponse.Yes : UserResponse.No;
            }

            Instance.Logger.Warn($"Consent for {templateID} is not found in ConsentMap. Returning Unknown");
            return UserResponse.Unknown;
        }

        /// <summary>
        /// Get consent for given TCF vendor's id and purpose ids
        /// </summary>
        /// <param name="vendorID">TCF vendor id</param>
        /// <param name="purposeIDs">purpose ids</param>
        /// <returns>is consent is given or not</returns>
        public static UserResponse GetConsent(int vendorID, params int[] purposeIDs)
        {
            if (!IsConsentRequiredInThisLocationFromCmp)
            {
                Instance.Logger.Debug($"Consent not required in this location from CMP. Returning Yes for vendor {vendorID} and purposes {string.Join(", ", purposeIDs)}");
                return UserResponse.Yes;
            }

            if (TcfMap == null)
            {
                Instance.Logger.Warn($"TcfMap is null. Returning Unknown for vendor {vendorID} and purposes {string.Join(", ", purposeIDs)}");
                return UserResponse.Unknown;
            }
            
            var vendorConsent = TcfMap.Core.VendorConsents.Contains(vendorID);
            var vendorLegitimateInterest = TcfMap.Core.VendorLegitimateInterests.Contains(vendorID);

            if (!vendorConsent && !vendorLegitimateInterest)
            {
                Instance.Logger.Warn($"Vendor {vendorID} isn't in the vendor consents list or vendor legitimate interests list. Returning No for vendor {vendorID} and purposes {string.Join(", ", purposeIDs)}");
                return UserResponse.No;
            }

            var result = false;
            foreach (var purposeID in purposeIDs)
            {
                if (vendorConsent && TcfMap.Core.PurposesConsents.Contains(purposeID))
                {
                    Instance.Logger.Debug($"Vendor {vendorID} has consent for purpose id {purposeID}");
                    result = true;
                }
                else if (vendorLegitimateInterest && TcfMap.Core.PurposesLegitimateInterests.Contains(purposeID))
                {
                    Instance.Logger.Debug($"Vendor {vendorID} has legitimate interest for purpose id {purposeID}");
                    result = true;
                }
                else
                {
                    Instance.Logger.Warn($"Vendor {vendorID} doesn't have consent or legitimate interest for purpose id {purposeID}");
                    result = false;
                    break;
                } 
            }
            
            Instance.Logger.Debug($"Returning {result.ToString()} for vendor {vendorID} and purposes {string.Join(", ", purposeIDs)}");
            return result ? UserResponse.Yes : UserResponse.No;
        }

        public static void ShowBasicConsentForm()
        {
            Service?.ShowBasicConsentForm();
        }

        public static void ShowAdvancedConsentForm()
        {
            Service?.ShowAdvancedConsentForm();
        }
    }
}