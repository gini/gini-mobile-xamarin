GiniBank SDK Xamarin Bindings and Examples
=================================================

This repository contains projects to help you use the Android and iOS GiniBank SDK  with Xamarin.

Requirements
-----------

##### Visual Studio Community 2019 for Mac
* Version >=8.10.13
* Mono Framework Runtime >=6.12.0.140

##### iOS
* Xcode >=13.1

##### Android
* SDK Tools Version: 26.1.1
* SDK Platform Tools Version: 31.0.3
* SDK Build Tools Version: 31.0.0
* Eclipse Temurin JDK >=1.8.0_302

Android
-------

#### Documentation
Take a look at the [Documentation](https://developer.gini.net/gini-mobile-android/bank-sdk/sdk/html/index.html) to see how to use GiniBank SDK for Android

#### Binding Libraries

Because modules depend on each other they need to be build in the right order:

1) Volley.Xamarin.Android
2) TrustKit.Xamarin.Android
3) GiniInternalCore.Xamarin.Android
4) GiniCaptureNetwork.Xamarin.Android
5) GiniCapture.Xamarin.Android
6) GiniBankApi.Xamarin.Android
7) GiniBank.Xamarin.AndroidExample

Only after that the example app can be run.

#### How to use in new project

1. Set Target Android version : 30

2. Add the DLLs created by next bindings libraries:
```
GiniBank.Xamarin.Android
GiniCapture.Xamarin.Android
GiniCaptureNetwork.Xamarin.Android
```
3. Add next NuGet library
```
Xamarin.GooglePlayServices.Vision
```
4. Add Camera permission to AndroidManifest.xml
```
<uses-permission android:name="android.permission.CAMERA" /
```

#### Customization

Customization of the Views is provided via overriding of app resources: dimensions, strings, colors, texts, etc. Take a look at the [Customization Guide](https://developer.gini.net/gini-mobile-android/bank-sdk/sdk/html/customization.html) to see which resources can be overridden.

#### Example Project

In project `ExampleAndroidApp` you can see how to work with GiniBank SDK

#### Troubleshooting

* "Error AMM0000: 	
```
Attribute application@label value=(....) from AndroidManifest.xml
is also present at AndroidManifest.xmlvalue=(@string/app_name)."
```
Suggestion: add 'tools:replace="android:label"' to <application> element at AndroidManifest.xml

* "Error APT2260: 
```
style attribute 'attr/colorOnPrimary (aka gini.exampleandroidapp:attr/colorOnPrimary)' not found. ..."
```
Suggestion: it can be happened after install NuGet Xamarin.GooglePlayServices.Vision. Just update all other Nugets to latest versions
  
#### Bindings updating
  
Time by time we have updates for our library of native Android GiniBank SDK
https://github.com/gini/gini-mobile-android
  
If you want update bindings project to latest version of native Android GiniBank SDK, just do next:
  
Check mvnrepository and download latest versions of GiniBank SDK files
  
For example look here<br />
https://mvnrepository.com/artifact/net.gini.android/gini-bank-sdk<br />
there is gini-bank-sdk-1.4.1.aar<br />
download it and put to project GiniBank.Xamarin.Android\Jars\gini-bank-sdk-1.4.1.aar<br />
**Warning**: remove old aar from project, for example prev version - 1.4.0 GiniBank.Xamarin.Android\Jars\gini-bank-sdk-1.4.0.aar<br />
  
Do the same steps for all bindings<br />
* GiniBank.Xamarin.Android -> https://mvnrepository.com/artifact/net.gini.android/gini-bank-sdk
* GiniBankApi.Xamarin.Android -> https://mvnrepository.com/artifact/net.gini.android/gini-bank-api-lib
* GiniCapture.Xamarin.Android -> https://mvnrepository.com/artifact/net.gini.android/gini-capture-sdk
* GiniCaptureNetwork.Xamarin.Android -> https://mvnrepository.com/artifact/net.gini.android/gini-capture-sdk-default-network
* GiniInternalCore.Xamarin.Android -> https://mvnrepository.com/artifact/net.gini.android/gini-internal-core-api-lib
* TrustKit.Xamarin.Android -> https://mvnrepository.com/artifact/com.datatheorem.android.trustkit/trustkit
* Volley.Xamarin.Android -> https://mvnrepository.com/artifact/com.android.volley/volley
  
  
**Warning**: If after updating you see any errors, pls take a look on this link to resolve all issues<br />
https://docs.microsoft.com/en-us/xamarin/android/platform/binding-java-library/troubleshooting-bindings
  

