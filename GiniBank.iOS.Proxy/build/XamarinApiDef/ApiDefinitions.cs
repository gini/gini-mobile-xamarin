using System;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace GiniBank.iOS
{
	// @interface AnalysisResultProxy : NSObject
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface AnalysisResultProxy
	{
		// @property (readonly, nonatomic, strong) ExtractionProxies * _Nonnull extractions;
		[Export ("extractions", ArgumentSemantic.Strong)]
		ExtractionProxies Extractions { get; }

		// -(instancetype _Nonnull)initWithExtractions:(ExtractionProxies * _Nonnull)extractions images:(id)images __attribute__((objc_designated_initializer));
		[Export ("initWithExtractions:images:")]
		[DesignatedInitializer]
		IntPtr Constructor (ExtractionProxies extractions, NSObject images);
	}

	// @interface ExtractionProxies : NSObject
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface ExtractionProxies
	{
	}

	// @interface ExtractionProxy : NSObject
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface ExtractionProxy
	{
		// @property (readonly, nonatomic, strong) BoxProxy * _Nullable box;
		[NullAllowed, Export ("box", ArgumentSemantic.Strong)]
		BoxProxy Box { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nullable candidates;
		[NullAllowed, Export ("candidates")]
		string Candidates { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nonnull entity;
		[Export ("entity")]
		string Entity { get; }

		// @property (copy, nonatomic) NSString * _Nonnull value;
		[Export ("value")]
		string Value { get; set; }

		// @property (copy, nonatomic) NSString * _Nullable name;
		[NullAllowed, Export ("name")]
		string Name { get; set; }

		// -(instancetype _Nonnull)initWithBox:(BoxProxy * _Nullable)box candidates:(NSString * _Nullable)candidates entity:(NSString * _Nonnull)entity value:(NSString * _Nonnull)value name:(NSString * _Nullable)name __attribute__((objc_designated_initializer));
		[Export ("initWithBox:candidates:entity:value:name:")]
		[DesignatedInitializer]
		IntPtr Constructor ([NullAllowed] BoxProxy box, [NullAllowed] string candidates, string entity, string value, [NullAllowed] string name);
	}

	// @interface GiniBankProxy_Swift_255 (ExtractionProxy)
	[Category]
	[BaseType (typeof(ExtractionProxy))]
	interface ExtractionProxy_GiniBankProxy_Swift_255
	{
	}

	// @interface BoxProxy : NSObject
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface BoxProxy
	{
		// @property (readonly, nonatomic) double height;
		[Export ("height")]
		double Height { get; }

		// @property (readonly, nonatomic) double left;
		[Export ("left")]
		double Left { get; }

		// @property (readonly, nonatomic) NSInteger page;
		[Export ("page")]
		nint Page { get; }

		// @property (readonly, nonatomic) double top;
		[Export ("top")]
		double Top { get; }

		// @property (readonly, nonatomic) double width;
		[Export ("width")]
		double Width { get; }

		// -(instancetype _Nonnull)initWithHeight:(double)height left:(double)left page:(NSInteger)page top:(double)top width:(double)width __attribute__((objc_designated_initializer));
		[Export ("initWithHeight:left:page:top:width:")]
		[DesignatedInitializer]
		IntPtr Constructor (double height, double left, nint page, double top, double width);
	}

	// @interface GiniCaptureProxy : NSObject
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface GiniCaptureProxy
	{
		// @property (readonly, nonatomic, strong) UIViewController * _Nonnull viewController;
		[Export ("viewController", ArgumentSemantic.Strong)]
		UIViewController ViewController { get; }

		// -(instancetype _Nonnull)initWithDomain:(NSString * _Nonnull)domain id:(NSString * _Nonnull)id secret:(NSString * _Nonnull)secret configuration:(GiniConfigurationProxy * _Nullable)configuration delegate:(id<GiniCaptureProxyDelegate> _Nonnull)delegate __attribute__((objc_designated_initializer));
		[Export ("initWithDomain:id:secret:configuration:delegate:")]
		[DesignatedInitializer]
		IntPtr Constructor (string domain, string id, string secret, [NullAllowed] GiniConfigurationProxy configuration, GiniCaptureProxyDelegate @delegate);
	}

	// @protocol GiniCaptureProxyDelegate
	[Protocol, Model]
	interface GiniCaptureProxyDelegate
	{
		// @required -(void)giniCaptureAnalysisDidFinishWithResult:(AnalysisResultProxy * _Nonnull)result sendFeedbackBlock:(void (^ _Nonnull)(ExtractionProxies * _Nonnull))sendFeedbackBlock;
		[Abstract]
		[Export ("giniCaptureAnalysisDidFinishWithResult:sendFeedbackBlock:")]
		void GiniCaptureAnalysisDidFinishWithResult (AnalysisResultProxy result, Action<ExtractionProxies> sendFeedbackBlock);

		// @required -(void)giniCaptureAnalysisDidFinishWithoutResults:(BOOL)showingNoResultsScreen;
		[Abstract]
		[Export ("giniCaptureAnalysisDidFinishWithoutResults:")]
		void GiniCaptureAnalysisDidFinishWithoutResults (bool showingNoResultsScreen);

		// @required -(void)giniCaptureDidCancelAnalysis;
		[Abstract]
		[Export ("giniCaptureDidCancelAnalysis")]
		void GiniCaptureDidCancelAnalysis ();
	}

	// @interface GiniConfigurationProxy : NSObject
	[BaseType (typeof(NSObject))]
	interface GiniConfigurationProxy
	{
		// @property (nonatomic) BOOL debugModeOn;
		[Export ("debugModeOn")]
		bool DebugModeOn { get; set; }

		// @property (nonatomic) enum GiniCaptureImportFileTypesProxy fileImportSupportedTypes;
		[Export ("fileImportSupportedTypes", ArgumentSemantic.Assign)]
		GiniCaptureImportFileTypesProxy FileImportSupportedTypes { get; set; }

		// @property (nonatomic) BOOL flashToggleEnabled;
		[Export ("flashToggleEnabled")]
		bool FlashToggleEnabled { get; set; }

		// @property (nonatomic) BOOL openWithEnabled;
		[Export ("openWithEnabled")]
		bool OpenWithEnabled { get; set; }

		// @property (nonatomic) BOOL qrCodeScanningEnabled;
		[Export ("qrCodeScanningEnabled")]
		bool QrCodeScanningEnabled { get; set; }

		// @property (nonatomic) BOOL multipageEnabled;
		[Export ("multipageEnabled")]
		bool MultipageEnabled { get; set; }

		// @property (nonatomic) BOOL onboardingShowAtFirstLaunch;
		[Export ("onboardingShowAtFirstLaunch")]
		bool OnboardingShowAtFirstLaunch { get; set; }

		// @property (nonatomic) BOOL onboardingShowAtLaunch;
		[Export ("onboardingShowAtLaunch")]
		bool OnboardingShowAtLaunch { get; set; }

		// @property (nonatomic, strong) UIColor * _Nullable navigationBarItemTintColor;
		[NullAllowed, Export ("navigationBarItemTintColor", ArgumentSemantic.Strong)]
		UIColor NavigationBarItemTintColor { get; set; }

		// @property (nonatomic, strong) UIColor * _Nullable navigationBarTintColor;
		[NullAllowed, Export ("navigationBarTintColor", ArgumentSemantic.Strong)]
		UIColor NavigationBarTintColor { get; set; }

		// @property (nonatomic, strong) UIColor * _Nullable navigationBarTitleColor;
		[NullAllowed, Export ("navigationBarTitleColor", ArgumentSemantic.Strong)]
		UIColor NavigationBarTitleColor { get; set; }

		// @property (nonatomic, strong) UIFont * _Nullable navigationBarItemFont;
		[NullAllowed, Export ("navigationBarItemFont", ArgumentSemantic.Strong)]
		UIFont NavigationBarItemFont { get; set; }

		// @property (nonatomic, strong) UIFont * _Nullable navigationBarTitleFont;
		[NullAllowed, Export ("navigationBarTitleFont", ArgumentSemantic.Strong)]
		UIFont NavigationBarTitleFont { get; set; }

		// @property (nonatomic, strong) UIColor * _Nullable documentPickerNavigationBarTintColor;
		[NullAllowed, Export ("documentPickerNavigationBarTintColor", ArgumentSemantic.Strong)]
		UIColor DocumentPickerNavigationBarTintColor { get; set; }

		// @property (nonatomic, strong) SimplePreferredButtonResource * _Nullable closeButtonResource;
		[NullAllowed, Export ("closeButtonResource", ArgumentSemantic.Strong)]
		SimplePreferredButtonResource CloseButtonResource { get; set; }

		// @property (nonatomic, strong) SimplePreferredButtonResource * _Nullable helpButtonResource;
		[NullAllowed, Export ("helpButtonResource", ArgumentSemantic.Strong)]
		SimplePreferredButtonResource HelpButtonResource { get; set; }

		// @property (nonatomic, strong) SimplePreferredButtonResource * _Nullable backToCameraButtonResource;
		[NullAllowed, Export ("backToCameraButtonResource", ArgumentSemantic.Strong)]
		SimplePreferredButtonResource BackToCameraButtonResource { get; set; }

		// @property (nonatomic, strong) SimplePreferredButtonResource * _Nullable backToMenuButtonResource;
		[NullAllowed, Export ("backToMenuButtonResource", ArgumentSemantic.Strong)]
		SimplePreferredButtonResource BackToMenuButtonResource { get; set; }

		// @property (nonatomic, strong) SimplePreferredButtonResource * _Nullable nextButtonResource;
		[NullAllowed, Export ("nextButtonResource", ArgumentSemantic.Strong)]
		SimplePreferredButtonResource NextButtonResource { get; set; }

		// @property (nonatomic, strong) SimplePreferredButtonResource * _Nullable cancelButtonResource;
		[NullAllowed, Export ("cancelButtonResource", ArgumentSemantic.Strong)]
		SimplePreferredButtonResource CancelButtonResource { get; set; }
	}

	// @interface GiniBankProxy_Swift_326 (GiniConfigurationProxy)
	[Category]
	[BaseType (typeof(GiniConfigurationProxy))]
	interface GiniConfigurationProxy_GiniBankProxy_Swift_326
	{
	}

	// @interface GiniSDKProxy : NSObject
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface GiniSDKProxy
	{
		// -(instancetype _Nonnull)initWithId:(NSString * _Nonnull)id secret:(NSString * _Nonnull)secret domain:(NSString * _Nonnull)domain __attribute__((objc_designated_initializer));
		[Export ("initWithId:secret:domain:")]
		[DesignatedInitializer]
		IntPtr Constructor (string id, string secret, string domain);

		// -(void)removeStoredCredentials;
		[Export ("removeStoredCredentials")]
		void RemoveStoredCredentials ();
	}

	// @interface SimplePreferredButtonResource : NSObject
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface SimplePreferredButtonResource
	{
		// @property (nonatomic, strong) UIImage * _Nullable preferredImage;
		[NullAllowed, Export ("preferredImage", ArgumentSemantic.Strong)]
		UIImage PreferredImage { get; set; }

		// @property (copy, nonatomic) NSString * _Nullable preferredText;
		[NullAllowed, Export ("preferredText")]
		string PreferredText { get; set; }

		// -(instancetype _Nonnull)initWithPreferredImage:(UIImage * _Nullable)preferredImage preferredText:(NSString * _Nullable)preferredText __attribute__((objc_designated_initializer));
		[Export ("initWithPreferredImage:preferredText:")]
		[DesignatedInitializer]
		IntPtr Constructor ([NullAllowed] UIImage preferredImage, [NullAllowed] string preferredText);
	}
}
