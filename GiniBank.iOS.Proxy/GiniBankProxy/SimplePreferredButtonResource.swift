//
//  SimplePreferredButtonResource.swift
//  GiniBankProxy
//
//  Created by Andrii Sorochak on 02.12.2021.
//

import UIKit
import Foundation
import GiniCaptureSDK

@objc(SimplePreferredButtonResource)
public class SimplePreferredButtonResource: NSObject, PreferredButtonResource {
    
    @objc public var preferredImage: UIImage?
    @objc public var preferredText: String?
    
    @objc public init(preferredImage: UIImage?, preferredText: String?) {
        
        self.preferredImage = preferredImage
        self.preferredText = preferredText
        
        super.init()
    }
}
