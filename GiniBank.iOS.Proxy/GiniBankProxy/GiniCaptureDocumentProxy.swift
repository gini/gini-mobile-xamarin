//
//  GiniCaptureDocumentProxy.swift
//  GiniBankProxy
//
//  Created by Andrii Sorochak on 13.12.2021.
//

import UIKit
import Foundation

@objc(GiniCaptureDocumentProxy)
public class GiniCaptureDocumentProxy: NSObject {

    @objc public var type: GiniCaptureDocumentTypeProxy
    @objc public var data: Data
    @objc public var id: String
    @objc public var previewImage: UIImage?
    @objc public var isReviewable: Bool
    @objc public var isImported: Bool
    
    @objc public init(type: GiniCaptureDocumentTypeProxy,
                      data: Data,
                      id: String,
                      previewImage: UIImage?,
                      isReviewable: Bool,
                      isImported: Bool) {
        
        self.type = type
        self.data = data
        self.id = id
        self.previewImage = previewImage
        self.isReviewable = isReviewable
        self.isImported = isImported
        
        super.init()
    }
}

@objc(GiniCaptureDocumentTypeProxy)
public enum GiniCaptureDocumentTypeProxy: Int {
    case pdf = 0
    case image = 1
    case qrcode = 2
}
