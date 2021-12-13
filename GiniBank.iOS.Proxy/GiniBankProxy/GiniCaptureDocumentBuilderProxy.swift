//
//  GiniCaptureDocumentBuilderProxy.swift
//  GiniBankProxy
//
//  Created by Andrii Sorochak on 13.12.2021.
//

import Foundation
import GiniBankSDK
import GiniCaptureSDK

@objc(GiniCaptureDocumentBuilderProxy)
public class GiniCaptureDocumentBuilderProxy: NSObject {
    
    @objc public func build(with openURL: URL, completion: @escaping (GiniCaptureDocumentProxy?) -> Void) {
        
        let documentBuilder = GiniCaptureDocumentBuilder(documentSource: .appName(name: nil))
        documentBuilder.importMethod = .openWith
        
        documentBuilder.build(with: openURL) { [weak self] (document) in
            
            guard let self = self else { return }

            if document == nil {
                completion(nil)
            }
            else
            {
                var documentTypeProxy = GiniCaptureDocumentTypeProxy.pdf
                switch document!.type {
                    case .pdf: documentTypeProxy = .pdf
                    case .image: documentTypeProxy = .image
                    case .qrcode: documentTypeProxy = .qrcode
                }
                
                let documentProxy = GiniCaptureDocumentProxy(
                    type: documentTypeProxy,
                    data: document!.data,
                    id: document!.id,
                    previewImage: document!.previewImage,
                    isReviewable: document!.isReviewable,
                    isImported: document!.isImported)
                
                completion(documentProxy)
            }
        }
    }
}
