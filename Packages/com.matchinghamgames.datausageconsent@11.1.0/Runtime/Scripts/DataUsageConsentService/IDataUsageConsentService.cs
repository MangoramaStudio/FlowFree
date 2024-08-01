using System;
using System.Collections.Generic;
using MatchinghamGames.Handyman;

namespace MatchinghamGames.DataUsageConsent
{
    public interface IDataUsageConsentService<out TDataUsageConsentServiceConfig> : IAsyncInitialize where TDataUsageConsentServiceConfig : IDataUsageConsentServiceConfig
    {
        #region Fields

        public bool Ready { get; }
        
        public TDataUsageConsentServiceConfig ServiceConfig { get; }

        public bool IsConsentRequiredInThisLocation { get; }
        
        public bool ShouldCollectConsent { get; }

        public event Action BasicConsentFormClosed;
        public event Action AdvancedConsentFormClosed;

        public event Action ConsentUpdated;
        
        public Area? CurrentArea { get; }
        public IDictionary<string, bool> ConsentMap { get; }
        
        public string TCString { get; }
        
        public string ACString { get; }
        
        public string CountryCode { get; }
        public string RegionCode { get; }
        
        #endregion

        #region Methods

        void Initialize();

        #endregion

        void ShowBasicConsentForm();
        
        void ShowAdvancedConsentForm();

        void LogTCFData();
        
        void LogACData();
    }
}