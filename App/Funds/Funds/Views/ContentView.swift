//
//  ContentView.swift
//  Funds
//
//  Created by richard Haynes on 7/27/25.
//

import SwiftUI

fileprivate struct FundRaiserResponse: Decodable {
    let fundRaisers: [FundRaiser]
}

struct ContentView: View {
    @State private var fundRaisers: [FundRaiser] = []
    @State private var isLoading = true
    @State private var errorMessage: String?
    
    private func fetchFundRaisers() async {
        isLoading = true
        errorMessage = nil
        defer { isLoading = false }

        guard let url = URL(string: "https://localhost:7204/api/fundraisers") else {
            errorMessage = "Invalid URL"
            return
        }

        do {
            let (data, _) = try await URLSession.shared.data(from: url)
            let decoder = JSONDecoder()
            decoder.dateDecodingStrategy = .iso8601
            let response = try decoder.decode(FundRaiserResponse.self, from: data)
            fundRaisers = response.fundRaisers
        } catch {
            errorMessage = error.localizedDescription
        }
    }
    var body: some View {
        NavigationView {
            Image(systemName: "globe")
                .imageScale(.large)
                .foregroundStyle(.tint)
            Text("Funds App!")
            
            VStack{
                if isLoading{
                    Text("....Loading")
                }
                else if let errorMessage = errorMessage {
                    Text(errorMessage).foregroundStyle(.red)
                }
                else {
                    List(fundRaisers) { fundRaiser in
                        VStack{
                            Text(fundRaiser.name).font(.headline)
                            Text("Current / Goal: $\(fundRaiser.currentAmount) / $\(fundRaiser.goalAmount)")
                            Text("Begin: \(fundRaiser.startDate)")
                            HStack {
                                NavigationLink("Make a Donation"){
                                    DonationView(fundraiser: fundRaiser)
                                }.buttonStyle(.borderedProminent)
                            }
                        }
                    }
                }
            }
            
            
        }.task {
            await fetchFundRaisers()
        }
        .padding()
    }
}

#Preview {
    ContentView()
}
