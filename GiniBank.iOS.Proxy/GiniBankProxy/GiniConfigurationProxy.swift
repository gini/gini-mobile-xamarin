//
//  GiniConfigurationProxy.swift
//  GiniBankProxy
//
//  Created by Andrii Sorochak on 01.12.2021.
//

import UIKit
import Foundation
import GiniCaptureSDK

@objc(GiniConfigurationProxy)
public class GiniConfigurationProxy: NSObject {
    
    @objc public var debugModeOn = false
    @objc public var fileImportSupportedTypes: GiniConfigurationProxy.GiniVisionImportFileTypesProxy = .none
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

extension GiniConfigurationProxy {
    
    @objc(GiniVisionImportFileTypesProxy)
    public enum GiniVisionImportFileTypesProxy: Int {
        case none
        case pdf
        case pdf_and_images
    }
}

extension GiniConfiguration.GiniCaptureImportFileTypes {
    
    init(giniVisionImportFileTypesProxy: GiniConfigurationProxy.GiniVisionImportFileTypesProxy) {
        
        switch giniVisionImportFileTypesProxy {
        case .none: self = .none
        case .pdf: self = .pdf
        case .pdf_and_images: self = .pdf_and_images
        }
    }
}

extension GiniConfiguration {
    
    convenience init(giniConfigurationProxy: GiniConfigurationProxy) {
        
        self.init()
        
        self.debugModeOn = giniConfigurationProxy.debugModeOn
        
        self.fileImportSupportedTypes = GiniConfiguration.GiniCaptureImportFileTypes(giniVisionImportFileTypesProxy: giniConfigurationProxy.fileImportSupportedTypes)
        self.flashToggleEnabled = giniConfigurationProxy.flashToggleEnabled
        self.openWithEnabled = giniConfigurationProxy.openWithEnabled
        self.qrCodeScanningEnabled = giniConfigurationProxy.qrCodeScanningEnabled
        self.multipageEnabled = giniConfigurationProxy.multipageEnabled
        self.onboardingShowAtFirstLaunch = giniConfigurationProxy.onboardingShowAtFirstLaunch
        self.onboardingShowAtLaunch = giniConfigurationProxy.onboardingShowAtLaunch
        
        if let navigationBarItemTintColor = giniConfigurationProxy.navigationBarItemTintColor {
            self.navigationBarItemTintColor = navigationBarItemTintColor
        }
        
        if let navigationBarTintColor = giniConfigurationProxy.navigationBarTintColor {
            self.navigationBarTintColor = navigationBarTintColor
        }
        
        if let navigationBarTitleColor = giniConfigurationProxy.navigationBarTitleColor {
            self.navigationBarTitleColor = navigationBarTitleColor
        }
        
        if let navigationBarTitleFont = giniConfigurationProxy.navigationBarTitleFont {
            self.navigationBarTitleFont = navigationBarTitleFont
        }
        
        if let documentPickerNavigationBarTintColor = giniConfigurationProxy.documentPickerNavigationBarTintColor {
            self.documentPickerNavigationBarTintColor = documentPickerNavigationBarTintColor
        }
        
        if let closeButtonResource = giniConfigurationProxy.closeButtonResource {
            self.closeButtonResource = closeButtonResource
        }
        
        if let helpButtonResource = giniConfigurationProxy.helpButtonResource {
            self.helpButtonResource = helpButtonResource
        }
        
        if let backToCameraButtonResource = giniConfigurationProxy.backToCameraButtonResource {
            self.backToCameraButtonResource = backToCameraButtonResource
        }
        
        if let backToMenuButtonResource = giniConfigurationProxy.backToMenuButtonResource {
            self.backToMenuButtonResource = backToMenuButtonResource
        }
        
        if let nextButtonResource = giniConfigurationProxy.nextButtonResource {
            self.nextButtonResource = nextButtonResource
        }
        
        if let cancelButtonResource = giniConfigurationProxy.cancelButtonResource {
            self.cancelButtonResource = cancelButtonResource
        }
        
        self.onboardingPages = giniConfigurationProxy.onboardingPages
    }
}
