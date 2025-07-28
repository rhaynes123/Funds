//
//  FundRaiser.swift
//  Funds
//
//  Created by richard Haynes on 7/27/25.
//

import Foundation
struct FundRaiser: Identifiable, Decodable {
    var id: UUID { UUID() } // Temporary ID for SwiftUI
    let name: String
    let goalAmount: Double
    let currentAmount: Decimal
    let startDate: String
    let endDate: String?
}
