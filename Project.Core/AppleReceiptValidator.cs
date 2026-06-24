using Newtonsoft.Json.Linq;
using Project.Core.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Project.Core
{
    public class AppleReceiptValidator
    {
        private static readonly HttpClient client = new HttpClient();

        // This method validates the receipt with Apple's server
        public static async Task<InAppValidateResponse> ValidateReceiptAsync(string receiptBase64, bool isSandbox)
        {
            InAppValidateResponse model = new() { IsValid = false };
            var endpoint = isSandbox
                ? "https://sandbox.itunes.apple.com/verifyReceipt"
                : "https://buy.itunes.apple.com/verifyReceipt";

            // Create the JSON request body
            var requestBody = new JObject
            {
                { "receipt-data", receiptBase64 },
                { "exclude-old-transactions", true},
                { "password", "e230beb51d1642d9bb6aff420d302161"}
            };

            var content = new StringContent(requestBody.ToString(), Encoding.UTF8, "application/json");

            try
            {
                // Send POST request to Apple server
                var response = await client.PostAsync(endpoint, content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var receiptResponse = JObject.Parse(jsonResponse);

                    int status = (int)receiptResponse["status"];
                    if (status == 0)
                    {
                        // The receipt is valid. Now you can check the subscription status
                        var latestReceiptInfo = receiptResponse["latest_receipt_info"];
                        foreach (var item in latestReceiptInfo)
                        {
                            if (!string.IsNullOrEmpty((string)item["expires_date_ms"]))
                            {
                                var expiresDate = (double)item["expires_date_ms"];
                                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(expiresDate / 1000));
                                model.ExpireDate = dateTimeOffset.DateTime;
                            }

                            // Check for active subscription or cancellation date
                            string cancellationDate = (string)item["cancellation_date"];
                            if (string.IsNullOrEmpty(cancellationDate))
                            {
                                // If there's no cancellation date, the subscription is active
                                Console.WriteLine("Subscription is active.");
                                model.IsValid = true;
                            }
                            else
                            {
                                // If there is a cancellation date, the subscription was canceled
                                Console.WriteLine("Subscription was canceled.");                                
                            }
                        }
                    }
                    else
                    {
                        // Handle error with receipt validation
                        Console.WriteLine($"Receipt validation failed with status: {status}");
                    }
                }
                else
                {
                    Console.WriteLine("Failed to contact Apple's verification server.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new InAppValidateResponse() { IsValid = false };
            }
            return model;
        }
    }
}
