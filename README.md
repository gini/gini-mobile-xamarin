# Gini Bank SDK Xamarin Bindings and Examples

This repository contains projects to help you use the Android and iOS Gini Bank SDK with Xamarin.

- [Gini Bank SDK Xamarin Bindings and Examples](#gini-bank-sdk-xamarin-bindings-and-examples)
  - [Requirements](#requirements)
    - [Visual Studio Community 2019 for Mac](#visual-studio-community-2019-for-mac)
    - [iOS](#ios)
    - [Android](#android)
  - [Android](#android-1)
    - [Documentation](#documentation)
    - [Binding Libraries](#binding-libraries)
    - [How to use in new project](#how-to-use-in-new-project)
    - [Customization](#customization)
    - [Example Project](#example-project)
    - [Updating the bindings](#updating-the-bindings)
  - [iOS](#ios-1)
    - [Example app](#example-app)
    - [Prerequisits](#prerequisits)
    - [Usage](#usage)
    - [Customization](#customization-1)
      - [Strings](#strings)
      - [Buttons](#buttons)
      - [Navigation Bar](#navigation-bar)
      - [Onboarding Pages](#onboarding-pages)
    - [Troubleshooting](#troubleshooting)
    - [Updating the `GiniBank.iOS.dll`](#updating-the-ginibankiosdll)

## Requirements

### Visual Studio Community 2019 for Mac
* Version >=8.10.13
* Mono Framework Runtime >=6.12.0.140

### iOS
* Xcode >=13.1
* [Objective Sharpie](https://docs.microsoft.com/en-us/xamarin/cross-platform/macios/binding/objective-sharpie/get-started) >=3.5 
 
### Android
* SDK Tools Version: 26.1.1
* SDK Platform Tools Version: 31.0.3
* SDK Build Tools Version: 31.0.0
* Eclipse Temurin JDK >=1.8.0_302

## Android

### Documentation

Take a look at the [Documentation](https://developer.gini.net/gini-mobile-android/bank-sdk/sdk/html/index.html) to see how to use GiniBank SDK for Android

### Binding Libraries

Because modules depend on each other they need to be build in the right order:

1) Volley.Xamarin.Android
2) TrustKit.Xamarin.Android
3) GiniInternalCore.Xamarin.Android
4) GiniCaptureNetwork.Xamarin.Android
5) GiniCapture.Xamarin.Android
6) GiniBankApi.Xamarin.Android
7) GiniBank.Xamarin.AndroidExample

Only after that the example app can be run.

### How to use in new project

1. Set Target Android version to: Android 11 (API Level 30)
2. Add the DLLs created by the following bindings libraries:  
   ```
   GiniBank.Xamarin.Android
   GiniCapture.Xamarin.Android
   GiniCaptureNetwork.Xamarin.Android
   ```
3. Add the following NuGet library:  
   ```
   Xamarin.GooglePlayServices.
   ```
4. Add Camera permission to AndroidManifest.xml:  
   ```
   <uses-permission android:name="android.permission.CAMERA" /
   ```

### Customization

Customization of the Views is provided via overriding of app resources: dimensions, strings, colors, texts, etc. Take a
look at the [Customization Guide](https://developer.gini.net/gini-mobile-android/bank-sdk/sdk/html/customization.html)
to see which resources can be overridden.

### Example Project

You can see how to work with GiniBank SDK in the `ExampleAndroidApp` project.

###Troubleshooting

* Error AMM0000: 	
   ```
   Attribute application@label value=(....) from AndroidManifest.xml
   is also present at AndroidManifest.xmlvalue=(@string/app_name)."
   ```
   Suggestion: add 'tools:replace="android:label"' to <application> element at AndroidManifest.xml

* Error APT2260: 
   ```
   style attribute 'attr/colorOnPrimary (aka gini.exampleandroidapp:attr/colorOnPrimary)' not found. ..."
   ```
   Suggestion: it can happen after installing the `Xamarin.GooglePlayServices.Vision` NuGet package. Just update all
   other Nugets to the latest versions.

### Updating the bindings

To update the bindings project to the latest version of our [native Android Gini Bank SDK](https://github.com/gini/gini-mobile-android/tree/main/bank-sdk) do the following steps:

1. Download latest version of the Android Gini Bank SDK and its dependencies from Maven Central:  
   For example to update to version 1.7.0 go
   [here](https://search.maven.org/artifact/net.gini.android/gini-bank-sdk/1.4.1/aar), click on `Downloads` and select
   `aar`.  
   Copy the aar to the `GiniBank.Xamarin.Android/Jars/` folder and it to the `GiniBank.Xamarin.Android` project in Visual Studio.  
   **Important**: don't forget to remove the old aar from the project.

Do the same steps for all bindings<br />
* GiniBank.Xamarin.Android -> https://search.maven.org/artifact/net.gini.android/gini-bank-sdk
* GiniBankApi.Xamarin.Android -> https://search.maven.org/artifact/net.gini.android/gini-bank-api-lib
* GiniCapture.Xamarin.Android -> https://search.maven.org/artifact/net.gini.android/gini-capture-sdk
* GiniCaptureNetwork.Xamarin.Android -> https://search.maven.org/artifact/net.gini.android/gini-capture-sdk-default-network
* GiniInternalCore.Xamarin.Android -> https://search.maven.org/artifact/net.gini.android/gini-internal-core-api-lib
* TrustKit.Xamarin.Android -> https://search.maven.org/artifact/com.datatheorem.android.trustkit/trustkit
* Volley.Xamarin.Android -> https://search.maven.org/artifact/com.android.volley/volley

**Note**: If after updating you see errors, please take check the Xamarin java binding [trooubleshooting page](https://docs.microsoft.com/en-us/xamarin/android/platform/binding-java-library/troubleshooting-bindings).

## iOS

GiniBank SDK for iOS is provided as a DLL file called `GiniBank.iOS.dll` located in `ExampleiOSApp`. It needs to be added to your project as a reference.

The API for the iOS integration is provided through a proxy library (`GiniBank.iOS.Proxy`) and it's a limited version of what's available natively. Please contact Gini if you need to access any functionality that isn't exposed. Only the `Screen API` is supported at this point.

### Example app

In order to run the example app on iOS, first add your `domain`, `id`, and `secret` to the `Credentials.plist`. Then you can run the app and test the extraction. The extractions are written to the console.

The example app is meant to provide a sample code to show how the GiniBank SDK can be used. Please see the `GiniBankSDKHelper.cs` in the `ExampleiOSApp` project to get you started.

### Prerequisits

In order to use the GiniBank SDK please ensure your project supplies the following:

* Keychan capability should be enabled in Entitlements.plist
* Camera Usage Description provided in the Info.plist
* Photo Library Usage Description provided in the Info.plist (if you want to enable loading photographs from the user's Photo app)

The app will crash at various points of execution if the above are not provided.

### Usage

Instantiate the `GiniCaptureProxy` and present the view controller accessible from the `ViewController` property:

```
GiniCaptureProxy gcProxy = new GiniCaptureProxy(domain, id, secret, _gConfiguration, _gcDelegate);
var gcViewController = gcProxy.ViewController;
_gcDelegate.GCViewController = gcViewController;
```

The `GiniCaptureProxy` initialiser takes your credentials (`domain`, `id`, and `secret`), a `GiniConfigurationProxy`
instance, and a `GiniCaptureDelegate` instance. 

The `GiniConfigurationProxy` allows you to configure different aspects of GiniBank SDK such as whether you want to
display a flash toggle button, or what tint should the navigation bar have. Please refer to the example app to see the
currently available options and to the native documentation for explanation of the options.

Implement the `GiniCaptureProxyDelegate` protocol in order to receive callbacks from GiniBank SDK with extraction
results or to be informed about lack of thereof.

### Customization

Components can be customized either through the `GiniConfiguration`, the `Localizable.strings` file or through the
assets. Take a look at the [Customization
Guide](https://developer.gini.net/gini-mobile-ios/GiniBankSDK/customization-guide.html) to see which resources can be
customized.

#### Strings

String customization is possible by adding GiniBank SDK's localized string keys with you own values to your
`Localizable.strings` files.

The example adds `ginicapture.navigationbar.camera.title`, `ginicapture.camera.qrCodeDetectedPopup.message` and
`ginicapture.camera.qrCodeDetectedPopup.buttonTitle` localized string keys and own values to both the German and the
English `Localizable.strings` file.

#### Buttons

Some buttons in the GiniBank SDK UI can be customized by setting their title or image. To do that, set a property on
yout `GiniConfigurationProxy` instance to a `SimplePreferredButtonResource`. The initaliser of
`SimplePreferredButtonResource` takes two agruments: `preferredImage` and `preferredText`. If you want the button to
have an icon, pass a `UIKit.UIImage` instance as the `preferredImage` and `null` as the `preferredText`. Pass your
desired button title as `preferredText` if you want the button to have a text title.

For instance in order to set the close button to have a text title instead of the default cross icon:

```
GiniConfigurationProxy gConfiguration = new GiniConfigurationProxy();
...
gConfiguration.CloseButtonResource = new SimplePreferredButtonResource(null, "Close please");
```

#### Navigation Bar

To configure the look of the navigation bars in Gini Bank SDK you should use the `GiniConfiguration` instead of
`UIAppearance`. This allows GiniBank SDK to prevent issues with the navigation bar colors in the
`UIDocumentPickerViewController`.

If you use `UIAppearance` you should reset navigation bar related customizations before launching GiniBank SDK and
restore them after GiniBank SDK has exited.

You can customize the following navigation bar and item properties:

```
GiniConfigurationProxy gConfiguration = new GiniConfigurationProxy
{
    NavigationBarItemTintColor = UIColor...,
    NavigationBarTintColor = UIColor...,
    NavigationBarTitleColor = UIColor...,
    NavigationBarItemFont = UIFont...,
    NavigationBarTitleFont = UIFont...,
    DocumentPickerNavigationBarTintColor = UIColor...,
};
```

#### Onboarding Pages

You can change the onboarding pages in two ways:
1. By adding your array of custom `UIView`s to `gConfiguration.OnboardingPages`.
2. By retrieving the default pages from `gConfiguration.OnboardingPages` then altering the order of the pages or
   remove/add pages. Pass the modified array to `gConfiguration.OnboardingPages`.

The buttons currently available for customization are:

| Asset name in native documentation | `GiniConfigurationProxy` property |
| ---------------------------------- | --------------------------------- |
| `navigationCameraClose`            | `CloseButtonResource`             |
| `navigationCameraHelp`             | `HelpButtonResource`              |
| `navigationReviewBack`             | `BackToCameraButtonResource`      |
| `navigationReviewBack`             | `BackToMenuButtonResource`        |
| `navigationReviewContinue`         | `NextButtonResource`              |
| `navigationAnalysisBack`           | `CancelButtonResource`            |

### Troubleshooting
* The app crashes at various points without an informative error message: please make sure that you have enabled all
  capabilities and provided all usage strings in your `Info.plist` file. 
* Document picker navigation bar buttons are not visible: please apply navigation bar customizations using the
  `GiniConfiguration` and reset your `UIAppearance` customizations related to navigation bars before launching GiniBank
  SDK. After GiniBank SDK has finished you can restore your `UIAppearance` customizations. 
  
### Updating the `GiniBank.iOS.dll`
  
1. Open `GiniBankProxy.xcodeproj` in XCode and build in 'iOS device' and 'iOS simulator' modes for release.
2. Go to `/Users/{username}/Library/Developer/Xcode/DerivedData` (you can also go to Xcode->Preferences->Locations-Derived Data to find the folder).
3. Copy the `Release-iphoneos` and `Release-iphonesimulator` folders from `.../DerivedData/GiniBankProxy(...)/Build/Products/` to `GiniBank.iOS.Proxy/build/`.
4. Run `bash build.sh`.  
   If it fails with `System.BadImageFormatException: Invalid Image` error then download and install Xamarin.iOS 15.2.0.1 from here: https://aka.ms/xvs/pkg/macios/15.2.0.1
5. The new `GiniBank.iOS.dll` will be generated and copied to the `ExampleiOSApp` folder.
