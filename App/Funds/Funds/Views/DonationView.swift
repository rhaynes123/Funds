//
//  DonationView.swift
//  Funds
//
//  Created by richard Haynes on 7/27/25.
//

import SwiftUI

struct DonationRequest: Encodable {
    let fundRaiserName: String
    let name: String
    let amount: Decimal
}

struct DonationView: View {
    let fundraiser: FundRaiser
    @Environment(\.dismiss) private var dismiss

    @State private var donationAmount = ""
    @State private var donorName: String = ""
    @State private var isSubmitting = false
    @State private var showConfirmation = false
    @State private var errorMessage: String?

    var body: some View {
        Form {
            Section(header: Text("Fundraiser")) {
                Text(fundraiser.name)
            }
            Section(header: Text("Your Name")) {
               TextField("Full Name", text: $donorName)
           }
            Section(header: Text("Donation Amount")) {
                TextField("Amount in USD", text: $donationAmount)
                   // .keyboardType(.decimalPad)
            }

            if let error = errorMessage {
                Section {
                    Text(error).foregroundColor(.red)
                }
            }

            Section {
                Button("Submit Donation") {
                    Task {
                        await submitDonation()
                    }
                }
                .disabled(isSubmitting || donationAmount.isEmpty)
            }
        }
        .navigationTitle("Donate")
        .alert("Thank You!", isPresented: $showConfirmation) {
            Button("OK") {
                dismiss()
            }
        } message: {
            Text("Your donation to \(fundraiser.name) has been submitted.")
        }
    }

    func submitDonation() async {
        guard let amount = Decimal(string: donationAmount) else {
            errorMessage = "Invalid amount"
            return
        }

        isSubmitting = true
        errorMessage = nil
        let donation = DonationRequest(
            fundRaiserName: fundraiser.name,
                    name: donorName,
                    amount: amount,
                )

        guard let url = URL(string: "https://localhost:7204/api/donate"),
              let data = try? JSONEncoder().encode(donation) else {
            errorMessage = "Failed to build request"
            isSubmitting = false
            return
        }

        var request = URLRequest(url: url)
        request.httpMethod = "POST"
        request.httpBody = data
        request.setValue("application/json", forHTTPHeaderField: "Content-Type")

        do {
            let (_, response) = try await URLSession.shared.data(for: request)
            if let httpResponse = response as? HTTPURLResponse, httpResponse.statusCode == 200 || httpResponse.statusCode == 201 {
                showConfirmation = true
            } else {
                errorMessage = "Failed with status code: \((response as? HTTPURLResponse)?.statusCode ?? 0)"
            }
        } catch {
            errorMessage = "Network error: \(error.localizedDescription)"
        }

        isSubmitting = false
    }
}

#Preview {
    DonationView(fundraiser: FundRaiser(name: "Test ", goalAmount: 23000_00, currentAmount: 300, startDate: "2025-01-01", endDate: nil))
}
