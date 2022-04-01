#!/bin/bash

# Fail on errors
set -e

modules="\
Volley.Xamarin.Android \
TrustKit.Xamarin.Android \
GiniInternalCore.Xamarin.Android \
GiniCaptureNetwork.Xamarin.Android \
GiniCapture.Xamarin.Android \
GiniBankApi.Xamarin.Android \
GiniBank.Xamarin.Android
"

# Remove all DLLs
rm ExampleAndroidApp/*.Android.dll || true

for module in $modules; do
  # Ensure it never expands to /bin by using ":?"
  rm -rf "${module:?}"/bin
done

# Build the top level binding which will also build all the bindings it depends on
msbuild GiniBank.Xamarin.Android/GiniBank.Xamarin.Android.csproj -target:Clean -property:Configuration=Debug
msbuild GiniBank.Xamarin.Android/GiniBank.Xamarin.Android.csproj -property:Configuration=Debug
msbuild GiniBank.Xamarin.Android/GiniBank.Xamarin.Android.csproj -target:Clean -property:Configuration=Release
msbuild GiniBank.Xamarin.Android/GiniBank.Xamarin.Android.csproj -property:Configuration=Release

# Copy the required DLLs to the example project
for module in $modules; do
  cp "${module}"/bin/Debug/"${module}".dll ExampleAndroidApp/
done

# Build the example project
msbuild ExampleAndroidApp/ExampleAndroidApp.csproj -target:Clean -property:Configuration=Debug
msbuild ExampleAndroidApp/ExampleAndroidApp.csproj -target:Build -property:Configuration=Debug
msbuild ExampleAndroidApp/ExampleAndroidApp.csproj -target:Clean -property:Configuration=Release
msbuild ExampleAndroidApp/ExampleAndroidApp.csproj -target:Build -property:Configuration=Release