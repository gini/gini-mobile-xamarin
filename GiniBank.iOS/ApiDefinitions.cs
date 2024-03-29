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
		// @property (readonly, copy, nonatomic) NSArray<UIImage *> * _Nonnull images;
		[Export ("images", ArgumentSemantic.Copy)]
		UIImage[] Images { get; }

		// @property (readonly, nonatomic, strong) ExtractionProxies * _Nonnull extractions;
		[Export ("extractions", ArgumentSemantic.Strong)]
		ExtractionProxies Extractions { get; }

		// -(instancetype _Nonnull)initWithExtractions:(ExtractionProxies * _Nonnull)extractions images:(NSArray<UIImage *> * _Nonnull)images __attribute__((objc_designated_initializer));
		[Export ("initWithExtractions:images:")]
		[DesignatedInitializer]
		IntPtr Constructor (ExtractionProxies extractions, UIImage[] images);
	}

	// @interface ExtractionProxies : NSObject
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface ExtractionProxies
	{
		// @property (readonly, copy, nonatomic) NSArray<ExtractionProxy *> * _Nonnull extractions;
		[Export ("extractions", ArgumentSemantic.Copy)]
		ExtractionProxy[] Extractions { get; }
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

	// @interface GiniBankProxy_Swift_262 (ExtractionProxy)
	[Category]
	[BaseType (typeof(ExtractionProxy))]
	interface ExtractionProxy_GiniBankProxy_Swift_262
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

	// @interface GiniCaptureDocumentBuilderProxy : NSObject
	[BaseType (typeof(NSObject))]
	interface GiniCaptureDocumentBuilderProxy
	{
		// -(void)buildWith:(NSURL * _Nonnull)openURL completion:(void (^ _Nonnull)(GiniCaptureDocumentProxy * _Nullable))completion;
		[Export ("buildWith:completion:")]
		void BuildWith (NSUrl openURL, Action<GiniCaptureDocumentProxy> completion);
	}

	// @interface GiniCaptureDocumentProxy : NSObject
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface GiniCaptureDocumentProxy
	{
		// @property (nonatomic) enum GiniCaptureDocumentTypeProxy type;
		[Export ("type", ArgumentSemantic.Assign)]
		GiniCaptureDocumentTypeProxy Type { get; set; }

		// @property (copy, nonatomic) NSData * _Nonnull data;
		[Export ("data", ArgumentSemantic.Copy)]
		NSData Data { get; set; }

		// @property (copy, nonatomic) NSString * _Nonnull id;
		[Export ("id")]
		string Id { get; set; }

		// @property (nonatomic, strong) UIImage * _Nullable previewImage;
		[NullAllowed, Export ("previewImage", ArgumentSemantic.Strong)]
		UIImage PreviewImage { get; set; }

		// @property (nonatomic) BOOL isReviewable;
		[Export ("isReviewable")]
		bool IsReviewable { get; set; }

		// @property (nonatomic) BOOL isImported;
		[Export ("isImported")]
		bool IsImported { get; set; }

		// -(instancetype _Nonnull)initWithType:(enum GiniCaptureDocumentTypeProxy)type data:(NSData * _Nonnull)data id:(NSString * _Nonnull)id previewImage:(UIImage * _Nullable)previewImage isReviewable:(BOOL)isReviewable isImported:(BOOL)isImported __attribute__((objc_designated_initializer));
		[Export ("initWithType:data:id:previewImage:isReviewable:isImported:")]
		[DesignatedInitializer]
		IntPtr Constructor (GiniCaptureDocumentTypeProxy type, NSData data, string id, [NullAllowed] UIImage previewImage, bool isReviewable, bool isImported);
	}

	// @interface GiniBankProxy : NSObject
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface GiniBankProxy
	{
		// @property (readonly, nonatomic, strong) UIViewController * _Nonnull viewController;
		[Export ("viewController", ArgumentSemantic.Strong)]
		UIViewController ViewController { get; }

		// -(instancetype _Nonnull)initWithDomain:(NSString * _Nonnull)domain id:(NSString * _Nonnull)id secret:(NSString * _Nonnull)secret configuration:(GiniConfigurationProxy * _Nullable)configuration delegate:(id<GiniCaptureProxyDelegate> _Nonnull)delegate importedDocument:(GiniCaptureDocumentProxy * _Nullable)importedDocument __attribute__((objc_designated_initializer));
		[Export ("initWithDomain:id:secret:configuration:delegate:importedDocument:")]
		[DesignatedInitializer]
		IntPtr Constructor (string domain, string id, string secret, [NullAllowed] GiniBankConfigurationProxy configuration, GiniCaptureProxyDelegate @delegate, [NullAllowed] GiniCaptureDocumentProxy importedDocument);
	}

	// @protocol GiniCaptureProxyDelegate
	[Protocol, Model (AutoGeneratedName = true)]
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

	// @interface GiniBankConfigurationProxy : NSObject
	[BaseType (typeof(NSObject))]
	interface GiniBankConfigurationProxy
	{
        // @property (nonatomic) BOOL enableReturnReasons;
        [Export("enableReturnReasons")]
        bool EnableReturnReasons { get; set; }

        // @property (nonatomic) BOOL returnAssistantEnabled;
        [Export("returnAssistantEnabled")]
        bool ReturnAssistantEnabled { get; set; }

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

		// @property (copy, nonatomic) NSArray<UIView *> * _Nonnull onboardingPages;
		[Export ("onboardingPages", ArgumentSemantic.Copy)]
		UIView[] OnboardingPages { get; set; }
	}

	// @interface GiniBankProxy_Swift_363 (GiniConfigurationProxy)
	[Category]
	[BaseType (typeof(GiniBankConfigurationProxy))]
	interface GiniBankConfigurationProxy_GiniBankProxy_Swift_363
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

		// +(void)receivePaymentRequestIdFromUrlWithUrl:(NSURL * _Nonnull)url onSuccess:(void (^ _Nonnull)(NSString * _Nonnull))onSuccess onFailure:(void (^ _Nonnull)(NSString * _Nonnull))onFailure;
		[Static]
		[Export ("receivePaymentRequestIdFromUrlWithUrl:onSuccess:onFailure:")]
		void ReceivePaymentRequestIdFromUrlWithUrl (NSUrl url, Action<NSString> onSuccess, Action<NSString> onFailure);

		// -(void)resolvePaymentRequestWithPaymentRequesId:(NSString * _Nonnull)paymentRequesId paymentInfo:(PaymentInfoProxy * _Nonnull)paymentInfo onSuccess:(void (^ _Nonnull)(ResolvedPaymentRequestProxy * _Nonnull))onSuccess onFailure:(void (^ _Nonnull)(NSString * _Nonnull))onFailure;
		[Export ("resolvePaymentRequestWithPaymentRequesId:paymentInfo:onSuccess:onFailure:")]
		void ResolvePaymentRequestWithPaymentRequesId (string paymentRequesId, PaymentInfoProxy paymentInfo, Action<ResolvedPaymentRequestProxy> onSuccess, Action<NSString> onFailure);

		// -(void)receivePaymentRequestWithPaymentRequesId:(NSString * _Nonnull)paymentRequesId onSuccess:(void (^ _Nonnull)(PaymentInfoProxy * _Nonnull))onSuccess onFailure:(void (^ _Nonnull)(NSString * _Nonnull))onFailure;
		[Export ("receivePaymentRequestWithPaymentRequesId:onSuccess:onFailure:")]
		void ReceivePaymentRequestWithPaymentRequesId (string paymentRequesId, Action<PaymentInfoProxy> onSuccess, Action<NSString> onFailure);

		// -(void)returnBackToBusinessAppHandlerWithResolvedPaymentRequest:(ResolvedPaymentRequestProxy * _Nonnull)resolvedPaymentRequest;
		[Export ("returnBackToBusinessAppHandlerWithResolvedPaymentRequest:")]
		void ReturnBackToBusinessAppHandlerWithResolvedPaymentRequest (ResolvedPaymentRequestProxy resolvedPaymentRequest);
	}

	// @interface PaymentInfoProxy : NSObject
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface PaymentInfoProxy
	{
		// @property (readonly, copy, nonatomic) NSString * _Nonnull recipient;
		[Export ("recipient")]
		string Recipient { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nonnull iban;
		[Export ("iban")]
		string Iban { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nullable bic;
		[NullAllowed, Export ("bic")]
		string Bic { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nonnull amount;
		[Export ("amount")]
		string Amount { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nonnull purpose;
		[Export ("purpose")]
		string Purpose { get; }

		// -(instancetype _Nonnull)initWithRecipient:(NSString * _Nonnull)recipient iban:(NSString * _Nonnull)iban bic:(NSString * _Nullable)bic amount:(NSString * _Nonnull)amount purpose:(NSString * _Nonnull)purpose __attribute__((objc_designated_initializer));
		[Export ("initWithRecipient:iban:bic:amount:purpose:")]
		[DesignatedInitializer]
		IntPtr Constructor (string recipient, string iban, [NullAllowed] string bic, string amount, string purpose);
	}

	// @interface ResolvedPaymentRequestProxy : NSObject
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface ResolvedPaymentRequestProxy
	{
		// @property (readonly, copy, nonatomic) NSString * _Nonnull requesterUri;
		[Export ("requesterUri")]
		string RequesterUri { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nonnull iban;
		[Export ("iban")]
		string Iban { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nullable bic;
		[NullAllowed, Export ("bic")]
		string Bic { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nonnull amount;
		[Export ("amount")]
		string Amount { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nonnull status;
		[Export ("status")]
		string Status { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nonnull purpose;
		[Export ("purpose")]
		string Purpose { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nonnull recipient;
		[Export ("recipient")]
		string Recipient { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nonnull createdAt;
		[Export ("createdAt")]
		string CreatedAt { get; }

		// -(instancetype _Nonnull)initWithRequesterUri:(NSString * _Nonnull)requesterUri iban:(NSString * _Nonnull)iban bic:(NSString * _Nullable)bic amount:(NSString * _Nonnull)amount status:(NSString * _Nonnull)status purpose:(NSString * _Nonnull)purpose recipient:(NSString * _Nonnull)recipient createdAt:(NSString * _Nonnull)createdAt __attribute__((objc_designated_initializer));
		[Export ("initWithRequesterUri:iban:bic:amount:status:purpose:recipient:createdAt:")]
		[DesignatedInitializer]
		IntPtr Constructor (string requesterUri, string iban, [NullAllowed] string bic, string amount, string status, string purpose, string recipient, string createdAt);
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
