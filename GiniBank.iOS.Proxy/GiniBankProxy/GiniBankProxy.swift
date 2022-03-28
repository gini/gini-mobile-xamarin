//
//  GiniBankProxy.swift
//  GiniBankProxy
//
//  Created by Andrii Sorochak on 01.12.2021.
//

import UIKit
import Foundation
import GiniBankSDK
import GiniCaptureSDK
import GiniBankAPILibrary

@objc(GiniSDKProxy)
public class GiniSDKProxy: NSObject {
    
    private let giniSDK: GiniBankAPI
    private let giniBank: GiniBank
    
    @objc public init(id: String, secret: String, domain: String) {
        
        let builder = GiniBankAPI.Builder(client: Client(id: id, secret: secret, domain: domain))
        
        giniSDK = builder.build()
        giniBank = GiniBank(with: giniSDK);
        
        super.init()
    }
        
    @objc public func removeStoredCredentials() {
        try? giniSDK.removeStoredCredentials()
    }
    
    @objc public static func receivePaymentRequestIdFromUrl(
        url: URL,
        onSuccess: @escaping (String) -> Void,
        onFailure: @escaping (String) -> Void) {
        
        receivePaymentRequestId(url: url) { result in
            switch result {
            case let .success(requestId):
                onSuccess(requestId);
            case let .failure(error):
                switch(error) {
                case .noRequestId:
                    onFailure("no RequestId");
                case .apiError(let apiError):
                    onFailure(apiError.message);
                case .amountParsingError(let amountString):
                    onFailure("amount parsing error: \(amountString)")
                }
            }
        }
    }
    
    @objc public func resolvePaymentRequest(
        paymentRequesId: String,
        paymentInfo: PaymentInfoProxy,
        onSuccess: @escaping (ResolvedPaymentRequestProxy) -> Void,
        onFailure: @escaping (String) -> Void) {
            
        let paymentInfo = PaymentInfo(
            recipient: paymentInfo.recipient,
            iban: paymentInfo.iban,
            bic: paymentInfo.bic,
            amount: paymentInfo.amount,
            purpose: paymentInfo.purpose)
            
        giniBank.resolvePaymentRequest(
            paymentRequesId: paymentRequesId,
            paymentInfo: paymentInfo) { result in
                    switch result {
                    case .success(let resolvedPaymentRequest):
                        let resolvedPaymentRequestProxy = ResolvedPaymentRequestProxy(
                            requesterUri: resolvedPaymentRequest.requesterUri,
                            iban: resolvedPaymentRequest.iban,
                            bic: resolvedPaymentRequest.bic,
                            amount: resolvedPaymentRequest.amount,
                            status: resolvedPaymentRequest.status,
                            purpose: resolvedPaymentRequest.purpose,
                            recipient: resolvedPaymentRequest.recipient,
                            createdAt: resolvedPaymentRequest.createdAt);
                        onSuccess(resolvedPaymentRequestProxy);
                    case .failure(let error):
                        switch(error) {
                        case .noRequestId:
                            onFailure("no RequestId");
                        case .apiError(let apiError):
                            onFailure(apiError.message);
                        case .amountParsingError(let amountString):
                            onFailure("amount parsing error: \(amountString)")
                        }
                    }
                }
          
    }
    
    @objc public func receivePaymentRequest(
        paymentRequesId: String,
        onSuccess: @escaping (PaymentInfoProxy) -> Void,
        onFailure: @escaping (String) -> Void) {
            
        giniBank.receivePaymentRequest(paymentRequestId: paymentRequesId) { result in
                        switch result {
                        case let .success(paymentRequest):
                            let paymentInfo = PaymentInfoProxy(
                                recipient: paymentRequest.recipient,
                                iban: paymentRequest.iban,
                                bic: paymentRequest.bic,
                                amount: paymentRequest.amount,
                                purpose: paymentRequest.purpose)
                            onSuccess(paymentInfo)
                        case let .failure(error):
                            switch(error) {
                            case .noRequestId:
                                onFailure("no RequestId");
                            case .apiError(let apiError):
                                onFailure(apiError.message);
                            case .amountParsingError(let amountString):
                                onFailure("amount parsing error: \(amountString)")
                            }
                        }
                    }
    }
    
    @objc public func returnBackToBusinessAppHandler(resolvedPaymentRequest:  ResolvedPaymentRequestProxy) {
       
        var bicJsonValue : String = "null";
        if resolvedPaymentRequest.bic != nil && resolvedPaymentRequest.bic?.isEmpty == false {
            bicJsonValue = "\"\(resolvedPaymentRequest.bic!)\"";
        }
        
        let json = """
            {
                "requesterUri": "\(resolvedPaymentRequest.requesterUri)",
                "iban": "\(resolvedPaymentRequest.iban)",
                "bic": \(bicJsonValue),
                "amount": "\(resolvedPaymentRequest.amount)",
                "status": "\(resolvedPaymentRequest.status)",
                "purpose": "\(resolvedPaymentRequest.purpose)",
                "recipient": "\(resolvedPaymentRequest.recipient)",
                "createdAt": "\(resolvedPaymentRequest.createdAt)"
            }
        """
        
        let jsonData = json.data(using: .utf8)!
        let decoder = JSONDecoder()
        let resolvedPaymentRequest = try! decoder.decode(ResolvedPaymentRequest.self, from: jsonData)
        
        giniBank.returnBackToBusinessAppHandler(
            resolvedPaymentRequest: resolvedPaymentRequest);
    }
}

@objc(ResolvedPaymentRequestProxy)
public class ResolvedPaymentRequestProxy: NSObject {
    @objc public let requesterUri: String
    @objc public let iban: String
    @objc public let bic: String?
    @objc public let amount: String
    @objc public let status: String
    @objc public let purpose: String
    @objc public let recipient: String
    @objc public let createdAt: String
    
    @objc public init(
        requesterUri: String,
        iban: String,
        bic: String?,
        amount: String,
        status: String,
        purpose: String,
        recipient: String,
        createdAt: String) {
        
        self.requesterUri = requesterUri
        self.iban = iban
        self.bic = bic
        self.amount = amount
        self.status = status
        self.purpose = purpose
        self.recipient = recipient
        self.createdAt = createdAt
        
        super.init()
    }
}

@objc(PaymentInfoProxy)
public class PaymentInfoProxy: NSObject {
    @objc public let recipient: String
    @objc public let iban: String
    @objc public let bic: String?
    @objc public let amount: String
    @objc public let purpose: String
    
    @objc public init(recipient: String, iban: String, bic: String?, amount: String, purpose: String) {
        
        self.recipient = recipient
        self.iban = iban
        self.bic = bic
        self.amount = amount
        self.purpose = purpose
        
        super.init()
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

@objc(GiniCaptureProxyDelegate)
public protocol GiniCaptureProxyDelegate {
    
    @objc func giniCaptureAnalysisDidFinishWith(result: AnalysisResultProxy,
                                               sendFeedbackBlock: @escaping (ExtractionProxies) -> Void)
    
    @objc func giniCaptureAnalysisDidFinishWithoutResults(_ showingNoResultsScreen: Bool)
    
    @objc func giniCaptureDidCancelAnalysis()
}

@objc(GiniCaptureProxy)
public class GiniCaptureProxy : NSObject {
    
    private var client: Client
    private let resultsDelegate: ResultsDelegate
    
    @objc public let viewController: UIViewController
    
    @objc
    public init(domain: String,
                id: String,
                secret: String,
                configuration: GiniConfigurationProxy?,
                delegate: GiniCaptureProxyDelegate,
                importedDocument: GiniCaptureDocumentProxy? = nil) {
        
        self.client = Client(id: id, secret: secret, domain: domain)
        self.resultsDelegate = ResultsDelegate()
        self.resultsDelegate.gcProxyDelegate = delegate
        
        let giniConfiguration: GiniConfiguration
        
        if let configuration = configuration {
            giniConfiguration = GiniConfiguration(giniConfigurationProxy: configuration)
        } else {
            giniConfiguration = GiniConfiguration()
        }
        
        if importedDocument == nil
        {
            self.viewController = GiniCapture.viewController(
                withClient: Client(id: id, secret: secret, domain: domain),
                configuration: giniConfiguration,
                resultsDelegate: resultsDelegate)
        }
        else {
            
            var importedDocuments = [GiniCaptureDocument]()
            
            let documentBuilder = GiniCaptureDocumentBuilder(documentSource: .appName(name: nil))
            documentBuilder.importMethod = .openWith
            
            let giniCaptureDocument = documentBuilder.build(with: importedDocument!.data)
            importedDocuments.append(giniCaptureDocument!)
           
            self.viewController = GiniCapture.viewController(
                withClient: Client(id: id, secret: secret, domain: domain),
                importedDocuments: importedDocuments,
                configuration: giniConfiguration,
                resultsDelegate: resultsDelegate)
        }
        
        super.init()
    }
}

private class ResultsDelegate: GiniCaptureResultsDelegate {
    
    weak var gcProxyDelegate: GiniCaptureProxyDelegate?
    
    public func giniCaptureAnalysisDidFinishWith(result: AnalysisResult,
                                                sendFeedbackBlock: @escaping ([String : Extraction]) -> Void) {
        
        let feedbackBlock: (ExtractionProxies) -> Void = { extractionProxies in
            
            var extractions = [String : Extraction]()
            
            for extractionProxy in extractionProxies.extractions {
                
                extractions[extractionProxy.entity] = Extraction(extractionProxy: extractionProxy)
            }
            
            sendFeedbackBlock(extractions)
        }
        
        gcProxyDelegate?.giniCaptureAnalysisDidFinishWith(result: AnalysisResultProxy(analysisResult: result),
                                                          sendFeedbackBlock: feedbackBlock)
    }
    
    public func giniCaptureAnalysisDidFinishWithoutResults(_ showingNoResultsScreen: Bool) {
        
        gcProxyDelegate?.giniCaptureAnalysisDidFinishWithoutResults(showingNoResultsScreen)
    }
    
    public func giniCaptureDidCancelAnalysis() {
        
        gcProxyDelegate?.giniCaptureDidCancelAnalysis()
    }
}
