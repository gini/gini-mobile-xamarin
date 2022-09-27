//
//  GiniConfigurationProxy.swift
//  GiniBankProxy
//
//  Created by Andrii Sorochak on 01.12.2021.
//

import UIKit
import Foundation
import GiniCaptureSDK

@objc(GiniBankConfigurationProxy)
public class GiniBankConfigurationProxy: NSObject {

    @objc public var enableReturnReasons = true
    @objc public var returnAssistantEnabled = true
    @objc public var debugModeOn = false
    @objc public var fileImportSupportedTypes: GiniBankConfigurationProxy.GiniCaptureImportFileTypesProxy = .none
    @objc public var flashToggleEnabled = false
    @objc public var openWithEnabled = false
    @objc public var qrCodeScanningEnabled = false
    @objc public var multipageEnabled = false
    @objc public var onboardingShowAtFirstLaunch = true
    @objc public var onboardingShowAtLaunch = true
    @objc public var navigationBarItemTintColor: UIColor?
    @objc public var navigationBarTintColor: UIColor?
    @objc public var navigationBarTitleColor: UIColor?
    @objc public var navigationBarItemFont: UIFont?
    @objc public var navigationBarTitleFont: UIFont?
    @objc public var documentPickerNavigationBarTintColor: UIColor?
    
    @objc public var closeButtonResource: SimplePreferredButtonResource?
    @objc public var helpButtonResource: SimplePreferredButtonResource?
    @objc public var backToCameraButtonResource: SimplePreferredButtonResource?
    @objc public var backToMenuButtonResource: SimplePreferredButtonResource?
    @objc public var nextButtonResource: SimplePreferredButtonResource?
    @objc public var cancelButtonResource: SimplePreferredButtonResource?
    
    @objc public var onboardingPages: [UIView] {
        get {
            if let pages = onboardingCustomPages {
                return pages
            }
            return GiniConfiguration().onboardingPages
        }
        set {
            self.onboardingCustomPages = newValue
        }
    }
    fileprivate var onboardingCustomPages: [UIView]?
}

extension GiniBankConfigurationProxy {
    
    @objc(GiniCaptureImportFileTypesProxy)
    public enum GiniCaptureImportFileTypesProxy: Int {
        case none
        case pdf
        case pdf_and_images
    }
}
