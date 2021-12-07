#!/usr/bin/env bash

# Open GiniBankProxy.xcodeproj in XCode and build in 'iOS device' and 'ios simulator' modes(release)
# Go to /Users/{username}/Library/Developer/Xcode/DerivedData(Xcode->Preferences->Locations-Derived Data)
# Copy folders Release-iphoneos and Release-iphonesimulator 
# from GiniBankProxy...../Build/Products/to GiniBank.iOS.Proxy/build folder

# clean build dir
rm -rf build/Release-fat
rm -rf build/Release-fat.lipo
rm -rf build/XamarinApiDef

# Init Fat Framework folder
cp -R build/Release-iphoneos build/Release-fat
cp -R build/Release-iphonesimulator/GiniBankProxy.framework/Modules/GiniBankProxy.swiftmodule build/Release-fat/GiniBankProxy.framework/Modules

# copy bundle res(swift packages issue)
cp -R build/Release-iphonesimulator/GiniBankSDK_GiniBankSDK.bundle build/Release-fat/GiniBankProxy.framework
cp -R build/Release-iphonesimulator/GiniCaptureSDK_GiniCaptureSDK.bundle build/Release-fat/GiniBankProxy.framework

# Create fat library
lipo -create build/Release-iphoneos/GiniBankProxy.framework/GiniBankProxy build/Release-iphonesimulator/GiniBankProxy.framework/GiniBankProxy -output build/Release-fat/GiniBankProxy.framework/GiniBankProxy

# Generate binding classes
sharpie bind --sdk=iphoneos --output="build/XamarinApiDef" --namespace="GiniBank.iOS"  --scope=build/Release-fat/GiniBankProxy.framework/Headers build/Release-fat/GiniBankProxy.framework/Headers/GiniBankProxy-Swift.h -c -F build/Release-fat
# TODO: fix 3 erros on missed Foundations(No extensions and onboardingPages)

# Fix binding issues

# Remove this line
sed '/using GiniBankProxy;/d' build/XamarinApiDef/APIDefinitions.cs > build/XamarinApiDef/APIDefinitions.cs_new
mv build/XamarinApiDef/APIDefinitions.cs_new build/XamarinApiDef/APIDefinitions.cs

# Fix nint to long issue
sed 's/nint/long/g' build/XamarinApiDef/StructsAndEnums.cs > build/XamarinApiDef/StructsAndEnums.cs_new
mv build/XamarinApiDef/StructsAndEnums.cs_new build/XamarinApiDef/StructsAndEnums.cs

# Copy to Bindings
cp build/XamarinApiDef/* ../GiniBank.iOS

# Build Bindings project
cd ../Bindings
msbuild ../GiniBank.iOS/GiniBank.iOS.csproj -t:Clean
msbuild ../GiniBank.iOS/GiniBank.iOS.csproj

# This will fail
msbuild

# Fix Xamarin pointlessly adding I to protocol names, but not everywhere.
sed 's/IGiniCaptureProxyDelegate/GiniCaptureProxyDelegate/g' ../GiniBank.iOS/obj/Debug/ios/GiniBank.iOS/GiniCaptureProxyDelegate.g.cs > ../GiniBank.iOS/obj/Debug/ios/GiniBank.iOS/GiniCaptureProxyDelegate.g.cs_new
mv ../GiniBank.iOS/obj/Debug/ios/GiniBank.iOS/GiniCaptureProxyDelegate.g.cs_new ../GiniBank.iOS/obj/Debug/ios/GiniBank.iOS/GiniCaptureProxyDelegate.g.cs

# This should succeed
msbuild ../GiniBank.iOS/GiniBank.iOS.csproj

cp ../GiniBank.iOS/bin/Debug/GiniBank.iOS.dll ../ExampleiOSApp/GiniBank.iOS.dll