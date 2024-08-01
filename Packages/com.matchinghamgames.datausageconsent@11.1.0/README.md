# Data Usage Consent

## Overview

`Data Usage Consent` package is used for enforcing privacy policy compliance and ATT/GDPR/CCPA consent acquisition. When a CMP service added, the module will use its API to get the user's consent. If no CMP service is added, the module will use its own prompt to get the user's consent

### Initialization Flow
![Initialiazaion Flow](/img~/initFlow.jpg)

## Installing
* Import this package using Unity's Package Manager UI.
* Import data usage consent service if you need to use CMP
  * Make sure that the data usage consent service configuration is correct.

## How to use
* Make sure to call `DataUsageConsentManager.Instance.Initialize()` at the appropriate place.
  * Analytics and mediation services needs user consent. **Thus, initialize it before everything else.**
* If you are using a CMP, make sure that you give the user the option to change their consent settings via `DataUsageConsentManager.ShowAdvancedConsentForm()`. e.g. a button in the settings menu.

## Configuration

Go to Matchingham > Data Usage Consent > Config to configure the module.

| Variable                   | Description                                                                                                                                          |
|----------------------------|------------------------------------------------------------------------------------------------------------------------------------------------------|
| Enabled                    | Enabling/disabling the module                                                                                                                        |
| AutoInitialize             | If enabled, you don't need to call Initialize method manually.                                                                                       |
| GeoLocationDetectionMethod | GeoLocation detection method, use either Google or Unity. Default is Google                                                                          |
| Default Popup Prefab       | In-game UI (prefab) that the player will see for prompt. See [Customizing the In-Game Prompt](#customizing-the-in-game-prompt)                       |
| Use Native Texts           | Use new Native popup texts instead of default ones                                                                                                   |
| Age Restriction            | Minimum age requirement writes on the default popup                                                                                                  |
| Localization Data Store    | DataUsageLocalizationData asset for localized texts. See [localization](#localization)                                                               |
| Pre ATT Popup Enabled      | (iOS Only) When enabled, a native popup will be opened before ATT request popup instead of default popup which is opened after the ATT request popup |
| Usage Description          | The prompt text that the player will see on ATT popup                                                                                                |

## Customizing the default popup

In game prompt is a prefab that is loaded on Android and iOS while initialization.
You can modify how this popup behaves by doing the following:
* You can create a variant of the same prefab in `Assets` folder and set that prefab as the prompt prefab
    * You can replace the script on the view to change/extend behaviour (New script must inherit from 
      `ConsentPromptViewBase` class or it will not work)
    * If your new prompt requires extra configuration that is not available on the settings object 
      of the package, you will need to create your own way of adding those configuration options. For this,
      you can simply create another `ScriptableObject` that holds your custom configuration options.
* Create a completely new prefab in `Assets` folder and set that as the prompt prefab in settings
    * You can attach either one of the existing prompt scripts that comes with the package, or create
    your own script, just make sure it inherits `ConsentPromptViewBase` class and correctly implements
    the API
    * If your new prompt requires extra configuration that is not available on the settings object
      of the package, you will need to create your own way of adding those configuration options. For this,
      you can simply create another `ScriptableObject` that holds your custom configuration options.
      
## API & Details

### Data Usage Consent Manager

#### Fields

* **Service**: Service of the module. See [services](#services)

* **CurrentArea**: User's current area.

* **IsATTRequired**: (iOS only) If ATT is required, this will return `true`

* *(iOS only)* **UserAdTracking**: This is directly mapped to user's ATT response
    * If user never gave any response to ATT prompt, this will return `Unknown`
    * If user has NOT given ATT consent, or given restricted consent, this will return `NotAllowed`
    * If user has given ATT consent, this will return `Allowed`

* **IsConsentRequiredInThisLocationFromCmp**: If the user is in a location where consent is required, this will return `true`. Works only if a CMP service is added. If you want to hide `Advanced Consent Form` button, you can use this field.

* **ShouldCollectConsentFromCmp**: If the user is in a location where consent is required and the user is required to give consent, this will return `true`. Works only if a CMP service is added.

* **CountryCode**: Returns the country code of the user's current location. Works only if a CMP service is added.

* **RegionCode**: Returns the region code of the user's current location. Works only if a CMP service is added.

#### Methods

* **SetUserConsent(UserResponse)**: Use this method to set user response from a prompt script. Only call this method from scripts that inherit from `ConsentPromptViewBase` class, and pass in user's response in the prompt as a parameter. You don't need to call this method anywhere if you are not using a custom prompt script, built-in prompts and logic already sets ad tracking response internally.

* **GetConsent(templateId: string)**: Use this method to get the consent status of a specific `Data Processing Service` template. If `Unknown` is returned, make sure that your configuration is correct.

* **GetConsent(vendrId: string, purposeIds: int[])**: Use this method to get the consent status of a specific `Vendor`'s `Purpose`(s). If `Unknown` is returned, make sure that your configuration is correct.

* **ShowBasicConsentForm()**: Use this method to show the basic consent form. This method will only work if a CMP service is added.

* **ShowAdvancedConsentForm()**: Use this method to show the advanced consent form. This method will only work if a CMP service is added.

#### Events

* **PreAttPopupClosed**: (iOS only) This event is invoked when the pre ATT popup is closed. This event is only invoked if `Pre ATT Popup Enabled` is enabled.

* **ConsentUpdated**: This event is invoked when the user's consent is updated. This event is only invoked if a CMP service is added.

* **BasicConsentFormClosed**: This event is invoked when the basic consent form is closed. This event is only invoked if a CMP service is added.

* **AdvancedConsentFormClosed**: This event is invoked when the advanced consent form is closed. This event is only invoked if a CMP service is added.

### Common

#### Methods

* **Initialize()**
  : Starts module initialization. You need to call this at the appropriate place.

* **WhenInitialized(Action callback)**
  : Allows you to register `callback` that will be fired only after the module is successfully initialized. Use this to execute logic that requires this module to be initialized first. If the module has already initialized, immediately invokes the callback.

* **WhenFailedToInitialize(Action callback)**
  : Allows you to register `callback` that will be fired only after the module fails to initialize for any reason. Use this to handle what should happen in case this module fails to initialize. If the module has already failed to initialize, immediately invokes the callback.

* **WhenReady(Action callback)**
  : Combined version of `WhenInitialized` and `WhenFailedToInitialize`. Delays execution of `callback` till module is first initialized or failed to initialize, immediately invoke the callback if it is already initialized or failed to initialize.

#### Fields

* **State**
  : Initialization state of the module

* **Instance**
  : Instance of the module

* **LateInitialized**
  : When the module needs an internet connection but the player became online while playing the game, this becomes `true`

* **Ready**
  : If the module is initialized successfully and ready to operate

* **Config**
  : Configuration of the module. See [configuration](#configuration)

* **InitializationDuration**
  : Initialization duration in seconds

## Localization 

This package is equipped with Localization Feature. To make use of it, you have to make sure the supported languages are selected.

To select languages; find "LanguageConfig" under "MatchinghamGames/Resources/Config/" path and change "Active Local Settings" field accordingly.

Make sure you select your "Default Font" so that this package can access it. If you would like, you can assign fallback fonts for different locales through "Special Font" field.

## Services

### Usercentrics

Usercentrics is a Consent Management Platform (CMP) that provides a way to collect user consent for data usage. This package uses Usercentrics SDK to collect user consent.
After installing the service, make sure that the configuration is correct.

#### Configuration

Go to Matcingham > Data Usage Consent > Usercentrics Config to configure the service.

| Variable            | Description                                                                            |
|---------------------|----------------------------------------------------------------------------------------|
| Settings ID iOS     | Usercentrics Setting ID for iOS, if set, **don't set** `Ruleset ID iOS` either         |
| Settings ID Android | Usercentrics Setting ID for Android, if set, **don't set** `Ruleset ID Android` either |
| Ruleset ID iOS      | Usercentrics Ruleset ID for iOS, if set, **don't set** `Setting ID iOS` either         |
| Ruleset ID Android  | Usercentrics Ruleset ID for Android, if set, **don't set** `Setting ID Android` either |
| Options             | Extra options for Usercentrics                                                         |

#### API & Details

##### Fields

* **Ready**: If the service is initialized successfully and ready to operate

* **ServiceConfig**: Configuration of the service. See [configuration](#configuration-1)

* **ConsentMap**: Returns the consent map. Key is the `templateID` of the `Data Processing Service` and value is the consent status of it.

* **IsConsentRequiredInThisLocation**: If the user is in a location where consent is required, this will return `true`.

* **ShouldCollectConsent**: If the user is in a location where consent is required and the user is required to give consent, this will return `true`.

* **CountryCode**: Returns the country code of the user's current location.

* **RegionCode**: Returns the region code of the user's current location.

* **CurrentArea**: User's current area.

##### Methods

* **Initialize()**: Starts service initialization.

* **ShowBasicConsentForm()**: Use this method to show the first layer.

* **ShowAdvancedConsentForm()**: Use this method to show the second layer.

* **LogTCFData()**: Use this method to log TCF data.

* **LogACData()**: Use this method to log AC data.

##### Events

* **ConsentUpdated**: This event is invoked when the user's consent is updated.

* **BasicConsentFormClosed**: This event is invoked when the first layer is closed.

* **AdvancedConsentFormClosed**: This event is invoked when the second layer is closed.

