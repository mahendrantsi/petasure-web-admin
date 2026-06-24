using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RestSharp;
using Project.Core.Enum;
using Project.Data.DBEntities;
using Project.Models;
using Project.Services.ServiceEntities;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks; 

namespace Project.Services.ServiceHelper
{
    public static class APIHelper
    {
        //public static async Task<HttpResponseMessage> GetAllInstitutionsAsync(ApiKeySetting key)
        //{
        //    try
        //    {
        //        var client = new HttpClient();
        //        var request = new HttpRequestMessage(HttpMethod.Get, $"{key.baseUrl}/{EnumApiType.institutions}");
        //        var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{key.applicationUuid}:{key.secret}"));
        //        request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");

        //        var response = await client.SendAsync(request);
        //        response.EnsureSuccessStatusCode();
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //public static async Task<HttpResponseMessage> GetInstitutionAsync(ApiKeySetting key, string institutionId)
        //{
        //    try
        //    {
        //        var client = new HttpClient();
        //        var request = new HttpRequestMessage(HttpMethod.Get, $"{key.baseUrl}/{EnumApiType.institutions}/{institutionId}");
        //        var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{key.applicationUuid}:{key.secret}"));
        //        request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");

        //        var response = await client.SendAsync(request);
        //        response.EnsureSuccessStatusCode();
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //public async static Task<HttpResponseMessage> CreatePaymentAuthorisation(ApiKeySetting key, string  jsonContent)
        //{
        //    try
        //    {
        //        var client = new HttpClient();
        //        var request = new HttpRequestMessage(HttpMethod.Post, $"{key.baseUrl}/payment-auth-requests");
        //        request.Headers.Add("psu-corporate-id", "string");
        //        request.Headers.Add("psu-id", "string");
        //        request.Headers.Add("psu-ip-address", "string");
        //        var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{key.applicationUuid}:{key.secret}"));
        //        request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");
        //        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        //        request.Content = content;
        //        var response = await client.SendAsync(request);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //public async static Task<HttpResponseMessage> CreateUser(ApiKeySetting key, string  email)
        //{
        //    try
        //    {
        //        var client = new HttpClient();
        //        var request = new HttpRequestMessage(HttpMethod.Post, $"{key.baseUrl}/{EnumApiType.users}");
        //        var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{key.applicationUuid}:{key.secret}"));
        //        request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");
        //        var content = new StringContent("{\r\n    \"applicationUserId\": \"john.doe@company.com\"\r\n  }", null, "application/json;charset=UTF-8");
        //        request.Content = content;
        //        var response = await client.SendAsync(request);
        //        response.EnsureSuccessStatusCode();
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //public async static Task<HttpResponseMessage> GetUser(ApiKeySetting key, string userUuid)
        //{
        //    try
        //    {
        //        var client = new HttpClient();
        //        var request = new HttpRequestMessage(HttpMethod.Get, $"{key.baseUrl}/{EnumApiType.users}/{userUuid}");
        //        var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{key.applicationUuid}:{key.secret}"));
        //        request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");
        //        var response = await client.SendAsync(request);
        //        response.EnsureSuccessStatusCode();
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        ////public async static Task<HttpResponseMessage> CreatePayment(ApiKeySetting key, string data)
        ////{
        ////    try
        ////    {
        ////        var client = new HttpClient();
        ////        var request = new HttpRequestMessage(HttpMethod.Post, $"{key.baseUrl}/{EnumApiType.payments}");
        ////        request.Headers.Add("consent", key.concentToken);
        ////        var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{key.applicationUuid}:{key.secret}"));
        ////        request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");
        ////        var content = new StringContent(data, Encoding.UTF8, "application/json");

        ////        request.Content = content;
        ////        var response = await client.SendAsync(request);
        ////        return response;
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        return null;
        ////    }
        ////}

        //public async static Task<HttpResponseMessage> PaymentStatus(ApiKeySetting key, string transactionID)
        //{
        //    try
        //    {
        //        var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{key.applicationUuid}:{key.secret}"));
        //        var httpClient = new HttpClient();
        //        using (var requestMessage = new HttpRequestMessage(System.Net.Http.HttpMethod.Get, $"{key.baseUrl}/{EnumApiType.payments}/{transactionID}/details"))
        //        { 
        //            requestMessage.Headers.Add("Authorization", $"Basic {base64authorization}");
        //            requestMessage.Headers.Add("Consent", key.concentToken);

        //            var response= await httpClient.SendAsync(requestMessage);
        //            return response;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
    }
}


