# Changelog

## [11.1.0] - 2024-03-25
### Added
* Privacy Policy Popup v5.1.x support
### Changed
* Privacy Policy Popup dependency: v5.0.0 -> v5.1.1
### Removed
* `Config.OpenPopupInDefaultPopup`: Configure it from `Privacy Policy Popup` package instead.

## [11.0.0] - 2024-03-19
### Added
* `IDataUsageConsentService`: `TCString`
* `IDataUsageConsentService`: `ACString`
* `Debugger`: `Vendor Consents`, `Vendor Legitimate Interests`, `Purpose Consents`, `Purpose Legitimate Interests`
* `GetConsent(vendorId: string, purposeIds: int[])`: Get consent for a specific vendor and purposes
* _Internal_ `TcfMap`: parsed TC string, using `Bidtellect.Tcf` library. Stored in stash.
* `Stash` dependency
### Changed
* `GetConsent(templateId: string)`: returns `UserResponse`instead of `bool`
* _Internal_ `ConsentUpdated` is invoked from module itself instead of service
* `ConsentMap` is internal now
* `ConsentMap` is stored in stash now
### Deprecated
* `UserConsent`: Use `GetConsent` method instead.
### Migration Guide
* Data Usage Consent Manager initialization depends on `Privacy Policy Popup` package. Make sure `Privacy Policy Popup` is initialized before. **If you are using `Game Utilities` package, UPDATE IT TO 5.1.0 OR NEWER. OTHERWISE, INITIALIZATION WILL STUCK.**
* If `GameAnalyticsMGSDKBridge` is installed, **UPDATE IT TO 4.1.0 OR NEWER. OTHERWISE, ATT POPUP WILL BE EARLIER THAN IT SHOULD.**
* If you using Usercentrics as CMP, install `Data Usage Consent Service - Usercentrics` from the Package Manager. Before that, don't forget to add "com.usercentrics" to scopes of the registry whose URL is "https://upm.matchingham.net" (commonly named as "MG Package Registry" or "Matchingham") and read the documentation.
* If you are using `Admost MGSDK Bridge`, update it to 6.1.0 or newer. Otherwise, you will get compile errors.
* If you are using `Sherlock Adjust Service`, update it to 6.3.0 or newer. Otherwise, you will get compile errors.
* When you are migrated to `Data Usage Consent Service - Usercentrics`. You can safely delete `Usercentrics` prefab from your main scene.

## [10.0.1] - 2024-03-11
### Fixed
* Compile error if Unity version is 2022.3 or older
### Migration Guide
* If you are migrated to `Data Usage Consent Service - Usercentrics`. You can safely delete `Usercentrics` prefab from your main scene. 

## [10.0.0] - 2024-03-11
### Added
* Service implementation (see `Migration Guide`): `IDataUsageConsentService`, `IDataUsageConsentServiceConfig`
* `Privact Policy Popup` initialization dependency (see `Migration Guide`)
* (iOS only) Pre-ATT and ATT request simulation on editor
* `IsATTRequired`: If the user is using iOS 14.5 or later, this will return true if the user has not yet been asked for permission to track.
* New methods, fields and events for service implementation (see `Documentation`)
* `Config.LocalizationDataStore` will be detected automatically if not set
### Changed
* Complete rework of the package (see `Migration Guide`)
* When CMP service is installed, internal geolocation detection will be disabled.
### Removed
* `UserCentrics` APIs (see `Migration Guide`)
* `IsCmpEverRequired`: Use `IsConsentRequiredInThisLocationFromCmp` instead.
* `CmpAnalytics`: Use `GetConsent` method instead.
* `OpenPrivacyPolicy()`, `OpenTermsAndConditions()`: Use `PrivacyPolicyPopup` APIs instead.
* `Config.CompanyName`: `ProjectSettings > Player > Company Name` will be used instead.
* `Config.PrivacyPolicyUrl`: `PrivacyPolicyPopup` configuration will be used instead.
* `Config.TermsAndConditionsUrl`: `PrivacyPolicyPopup` configuration will be used instead.
* `GameAnalyticsMGSDKBridge` initialization dependency (see `Migration Guide`)
### Fixed
* Rare exception while default popup opening.
### Deprecated
* `IsGDPRRequired`, `IsCCPARequired`: Use `CurrentArea` instead. 
### Migration Guide
* Configuration from previous versions will be **invalid**. Don't forget to read the documentation and reconfigure the package.
* Data Usage Consent Manager initialization depends on `Privacy Policy Popup` package. Make sure `Privacy Policy Popup` is initialized before. **If you are using `Game Utilities` package, UPDATE IT TO 5.1.0 OR NEWER. OTHERWISE, INITIALIZATION WILL STUCK.**
* If `GameAnalyticsMGSDKBridge` is installed, **UPDATE IT TO 4.1.0 OR NEWER. OTHERWISE, ATT POPUP WILL BE EARLIER THAN IT SHOULD.**
* If you using Usercentrics as CMP, install `Data Usage Consent Service - Usercentrics` from the Package Manager. Before that, don't forget to add "com.usercentrics" to scopes of the registry whose URL is "https://upm.matchingham.net" (commonly named as "MG Package Registry" or "Matchingham") and read the documentation.

## [9.4.0] - 2024-02-21
### Removed
* `UserCentrics` files from the package. See `Migration Guide`
### Changed
* Handyman dependency: v5.0.0 -> v5.3.1
* Worldwide dependency: v4.0.0 -> v4.1.0
* Logger dependency: v2.0.0 -> v2.1.0
* Privacy Policy Popup dependency: v4.0.1 -> v5.0.0
### Migration Guide
* If you want to use `UserCentrics` as CMP, you must install `Usercentrics Unity SDK` manually from the Package Manager. If couldn't locate the package, add "com.usercentrics" to scopes of the registry whose URL is "https://upm.matchingham.net" (commonly named as "MG Package Registry" or "Matchingham")

## [9.3.2] - 2024-02-01
### Fixed
* Fix for Native Pop Up texts are not readable enough if IOS Dark Mode is enabled

## [9.3.1] - 2024-01-26
### Fixed
* Fix for crashes on IOS 12.5 devices
### Added
* New undocumented config features added to the Documentation Page

## [9.3.0] - 2024-01-12
### Added
* Expose DUCM integrated privacy policy link behaviour (OpenPrivacyPolicy())
* Expose DUCM integrated terms and conditions link behaviour (OpenTermsAndConditions())

## [9.2.2] - 2023-01-09
### Fixed
* [Optional-Minor] IsGDPRRequired condition check was added for IsCmpEverRequired condition.

## [9.2.1] - 2023-01-05
### Fixed
* UserResponse dependent initialization results in failure due to premature ending of initialization. (Admost etc)

## [9.2.0] - 2023-01-04
### Added
* IOS Support for CMP.

### Changed
* Removed Package Load flow from Usercentrics, just enabling is enough.
* Updated userCentrics 2.10.3 > 2.11.0

### Migration Guide
* If you are updating your project from v9.0.0 or v9.0.1, you should remove Assets/UserCentrics folder before update.
* GameAnalytics is incompatible with this version. Please remove any gameanalytics packages and related define symbols from your project.
* [ANDROID] Please do not forget to resolve after the update.

## [9.1.0] - 2023-01-02
### Removed
* Reverted the changes in v9.0.1-prev.1

### Fixed
* IOS Ruleset ID and Android Ruleset IDs are separated.

## [9.0.1-prev.1] - 2023-01-02
### Added
* UserCentrics CMP PreATT Popup Support.

## [9.0.0] - 2023-12-22
### Added
* UserCentrics CMP Support.

### Changed
* Old prefabs removed
* Default values for 'UseNativeText' and 'PreATTPopup' are set to true
* Privacy Policy Popup dependency added. Make sure Privacy Policy Popup is configured before enabling Ingame Popups with 'InGamePopUp' key
* 'InGamePopUp' and 'UseNativeTexts' keys added with default values 'false'.
* Set 'UseNativeTexts' to 'true' if you will use Native Consent Texts in Consent Popup.

### Removed
* Google UMP is no longer supported.

### Fixed
* UserCentrics instance is destroyed at scene change bug.

## [9.0.0-prev.3] - 2023-12-19
### Added
* UserCentrics CMP Support.

### Removed
* Google UMP is deprecated.

## [9.0.0-prev.2] - 2023-12-14
### Changed
* Old prefabs removed
* Default values for 'UseNativeText' and 'PreATTPopup' are set to true

## [9.0.0-prev.1] - 2023-12-14
### Changed
* Privacy Policy Popup dependency added. Make sure Privacy Policy Popup is configured before enabling Ingame Popups with 'InGamePopUp' key
* 'InGamePopUp' and 'UseNativeTexts' keys added with default values 'false'.
* Set 'UseNativeTexts' to 'true' if you will use Native Consent Texts in Consent Popup.

## [8.0.3] - 2023-12-11
### Fixed
* Worldwide dependency: v3.0.0 -> v4.0.0

## [8.0.2] - 2023-12-07
### Fixed
* Dependency error fix

## [8.0.1] - 2023-11-29
### Fixed
* Compile errors

## [8.0.0] - 2023-11-28
### Added
* Unity 2023.2.x compatibility (see `Migration Guide`)
### Changed
* Logger 2.x implementation (see `Migration Guide`)
* Minimum Unity version is now 2021.3
* Handyman dependency: v4.1.5 -> v5.0.0
* Worldwide dependency: v2.1.4 -> v3.0.0
### Removed
* `LogFlags`, `OverrideLogFlagsToAllOnDebug`: With Logger 2.x update, all log levels are moved to Logger.Config
### Migration Guide
* With Logger v2.x update, we are setting a milestone here. If any other packages depend on Logger v1.x, you will get compile errors. Please update all packages.
* TextMeshPro dependency:
  * Unity 2023.2 or newer users: TextMeshPro package is deprecated. Unity UI v2+ package includes TextMeshPro. It is necessary to remove TextMeshPro package.
  * Unity 2023.1 and older users: TextMeshPro package is no longer a dependency, but it is necessary to install to use this package properly.

## [7.0.0] - 2023-11-28
### Added
* Pre-ATT PopUp for IOS
* EEA detection options added. You can use either Google or Unity
### Changed
* Consent message changed

## [6.3.0-prev.2] - 2023-11-03
### Added
* Validation checks
### Fixed
* Doc path updates

## [6.3.0-prev.1] - 2023-10-25
### Fixed
* No Internet Case check, do not attempt CMP flow for it might get stuck

## [6.3.0-prev.0] - 2023-10-19
### Changed
* Use default privacy policy flow outside EEA (GDPR countries)

## [6.2.0] - 2023-07-26
### Added
* Google User Messaging Platform revocation support. Check Docs.
  * I adherence to GDPR IAB, we are providing an API to allow the app
    to present the user with a consent form, after the initial consent prompt.

## [6.1.0] - 2023-07-26
### Added
* Google User Messaging Platform support.
  * See Docs for details and how to use.

## [6.0.2] - 2023-02-27
### Fix
* Romanian Language special symbol fix

## [6.0.1] - 2023-01-17
### Fixed
* Compile fix on `DataConsentLocalizedView`

## [6.0.0] - 2023-01-09
### Fixed
* Compile fix on `DataConsentLocalizedView`
### Added
* New Debugger implementation
### Removed
* `ConsentRequirementDelegate`

## [5.1.4] - 2022-11-25
### Feature
* Company name and age restriction can be given at config.
* Fixed the not compressed button texture.

## [5.1.3] - 2022-11-21
### Fixed
* Localization module minor bugfix.

## [5.1.2] - 2022-11-20
### Changed
* Documented Localization Feature, some minor UI fixes.

## [5.1.1] - 2022-10-03
### Changed
* WorldWide Localization Update for Font change logic.

## [5.1.0] - 2022-10-02
### Changed
* Localization feature.

## [5.0.1] - 2022-09-19
### Changed
* Better initialization when `GameAnalytics` is present

## [5.0.0] - 2022-09-01
### Changed
* Code refactor with `Handyman`: All public fields and methods are static now.

## [4.0.0] - 2022-07-27
### Changed
* User Prompt rework

## [3.0.0] - 2022-07-05
### Changed
* User Prompt rework
* Initialization rework: `RequestUserConsent` is obsolete and will be removed in next release. User prompt will be instantiated  on initialization.

## [2.0.1] - 2022-06-20
### Fixed
* `Privacy Policy Popup` module dependency bump

## [2.0.0] - 2022-06-17
### Changed
* **Consent flow rework**: Privacy Policy prompt will always be given to the user at first session. 

## [1.4.0] - 2022-06-10
### Changed
* *Internal* code refactor
### Fixed
* Stuck on initialization

## [1.3.0-preview.5] - 2022-06-06
### Changed
* Privacy Policy Popup dependency bump

## [1.3.0-preview.3] - 2022-06-02
### Fixed
* Fixed exception when installing the package

## [1.3.0-preview.2] - 2022-06-02
### Changed
* GA Bridge package dependency

## [1.3.0-preview] - 2022-05-17
### Fixed
* Tap handling on links
### Added
* Web request timeout

## [1.2.1] - 2022-05-13
### Fixed
* Privacy Policy Popup dependency

## [1.2.0] - 2022-04-20
### Added
* Landscape mode
* New ui elements

## [1.1.11] - 2022-03-17
### Fixed
* Privacy Policy package dependency updated

## [1.1.10] - 2022-03-10
### Fixed
* Compile fix on iOS targets
* Namespace on asmdef files

## [1.1.9] - 2022-03-09
* Initial release
