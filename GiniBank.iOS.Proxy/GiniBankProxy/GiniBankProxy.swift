//
//  GiniBankProxy.swift
//  GiniBankProxy
//
//  Created by Andrii Sorochak on 01.12.2021.
//

import UIKit
import Foundation
import GiniCaptureSDK
import GiniBankAPILibrary

@objc(GiniSDKProxy)
public class GiniSDKProxy: NSObject {
    
    private let giniSDK: GiniBankAPI
    
    @objc public init(id: String, secret: String, domain: String) {
        
        let builder = GiniBankAPI.Builder(client: Client(id: id, secret: secret, domain: domain))
        
        giniSDK = builder.build()
        
        super.init()
    }
        
    @objc public func removeStoredCredentials() {
        try? giniSDK.removeStoredCredentials()
    }
}

@objc(AnalysisResultProxy)
public class AnalysisResultProxy: NSObject {
    
    @objc public let images: [UIImage]
    
    @objc public let extractions: ExtractionProxies
    
    @objc public init(extractions: ExtractionProxies, images: [UIImage]) {
        self.extractions = extractions
        self.images = images
        
        super.init()
    }
    
    convenience init(analysisResult: AnalysisResult) {
        
        let extractionProxies = analysisResult.extractions.map { ExtractionProxy(extraction: $0.value) }
        self.init(extractions: ExtractionProxies(extractions: extractionProxies), images: analysisResult.images)
    }
}

@objc(GVLProxyDelegate)
public protocol GVLProxyDelegate {
    
    @objc func giniVisionAnalysisDidFinishWith(result: AnalysisResultProxy,
                                               sendFeedbackBlock: @escaping (ExtractionProxies) -> Void)
    
    @objc func giniVisionAnalysisDidFinishWithoutResults(_ showingNoResultsScreen: Bool)
    
    @objc func giniVisionDidCancelAnalysis()
}

@objc(GVLProxy)
public class GVLProxy : NSObject {
    
    private var client: Client
    private let resultsDelegate: ResultsDelegate
    
    @objc public let viewController: UIViewController
    
    @objc
    public init(domain: String,
                id: String,
                secret: String,
                configuration: GiniConfigurationProxy?,
                delegate: GVLProxyDelegate) {
        
        self.client = Client(id: id, secret: secret, domain: domain)
        self.resultsDelegate = ResultsDelegate()
        self.resultsDelegate.gvlProxyDelegate = delegate
        
        let giniConfiguration: GiniConfiguration
        
        if let configuration = configuration {
            giniConfiguration = GiniConfiguration(giniConfigurationProxy: configuration)
        } else {
            giniConfiguration = GiniConfiguration()
        }
        
        self.viewController = GiniCapture.viewController(withClient: Client(id: id, secret: secret, domain: domain),
                                                        configuration: giniConfiguration,
                                                        resultsDelegate: resultsDelegate)
        
        super.init()
    }
}

private class ResultsDelegate: GiniCaptureResultsDelegate {
    
    weak var gvlProxyDelegate: GVLProxyDelegate?
    
    public func giniCaptureAnalysisDidFinishWith(result: AnalysisResult,
                                                sendFeedbackBlock: @escaping ([String : Extraction]) -> Void) {
        
        let feedbackBlock: (ExtractionProxies) -> Void = { extractionProxies in
            
            var extractions = [String : Extraction]()
            
            for extractionProxy in extractionProxies.extractions {
                
                extractions[extractionProxy.entity] = Extraction(extractionProxy: extractionProxy)
            }
            
            sendFeedbackBlock(extractions)
        }
        
        gvlProxyDelegate?.giniVisionAnalysisDidFinishWith(result: AnalysisResultProxy(analysisResult: result),
                                                          sendFeedbackBlock: feedbackBlock)
    }
    
    public func giniCaptureAnalysisDidFinishWithoutResults(_ showingNoResultsScreen: Bool) {
        
        gvlProxyDelegate?.giniVisionAnalysisDidFinishWithoutResults(showingNoResultsScreen)
    }
    
    public func giniCaptureDidCancelAnalysis() {
        
        gvlProxyDelegate?.giniVisionDidCancelAnalysis()
    }
}
