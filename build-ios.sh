#!/bin/bash
# 
# Builds the GiniBank.iOS.dll.
#
# Check the README for required preconditions.
#

# Uncomment for debugging (prints the executed commands)
# set -x

pushd GiniBank.iOS.Proxy

# clean build dir
rm -rf build/Release-fat
rm -rf build/Release-fat.lipo
rm -rf build/XamarinApiDef

# Init Fat Framework folder
cp -R build/Release-iphoneos build/Release-fat
cp -R build/Release-iphonesimulator/GiniBankProxy.framework/Modules/GiniBankProxy.swiftmodule build/Release-fat/GiniBankProxy.framework/Modules

# copy bundle res(swift packages issue)
cp -R build/Release-iphoneos/GiniBankSDK_GiniBankSDK.bundle build/Release-fat/GiniBankProxy.framework
cp -R build/Release-iphoneos/GiniCaptureSDK_GiniCaptureSDK.bundle build/Release-fat/GiniBankProxy.framework

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

# Revert .NET 6 NativeHandle to previous IntPtr type so that it works with the bindings project (https://github.com/xamarin/xamarin-macios/issues/13087)
sed 's/NativeHandle/IntPtr/g' build/XamarinApiDef/APIDefinitions.cs > build/XamarinApiDef/APIDefinitions.cs_new
mv build/XamarinApiDef/APIDefinitions.cs_new build/XamarinApiDef/APIDefinitions.cs

# Copy to Bindings
cp build/XamarinApiDef/* ../GiniBank.iOS

# Build Bindings project
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

popd
