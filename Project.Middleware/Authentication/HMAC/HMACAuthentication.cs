using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Project.Middleware.Authentication.HMAC
{

    // public class HMACAuthentication : Attribute, IAuthenticationFilter
    // {
    //     // / <summary>
    //     // / 
    //     // / </summary>
    //     public HMACAuthenticationAttribute()
    //     {
    //         Logger.CustomLogger.LogMessage("HMACAuthenticationAttribute()");
    //         APIAccessIndexer.Init();
    //     }
    //     // / <summary>
    //     // / 
    //     // / </summary>
    //     // / <param name="context"></param>
    //     // / <param name="cancellationToken"></param>
    //     // / <returns></returns>
    //     public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
    //     {
    //         Logger.CustomLogger.LogMessage("AuthenticateAsync ExecuteAsync line 45");
    //         var req = context.Request;
    //         if (IsHAMCCheckon(context))
    //         {
    //             Logger.CustomLogger.LogMessage("AuthenticateAsync ExecuteAsync line 48");
    //             if (req.Headers.Authorization != null && SiteSettings.HMACAuthenticationScheme.Equals(req.Headers.Authorization.Scheme, StringComparison.OrdinalIgnoreCase))
    //             {
    //                 var rawAuthzHeader = req.Headers.Authorization.Parameter;
    //                 var authorizationHeaderArray = CommonHelper.GetAuthorizationHeaderValues(rawAuthzHeader);
    //                 if (authorizationHeaderArray != null)
    //                 {
    //                     var clientId = authorizationHeaderArray["organizationid"];
    //                     var bodyhash = authorizationHeaderArray["bodyhash"];
    //                     var mac = authorizationHeaderArray["mac"];
    //                     var nonce = authorizationHeaderArray["nonce"];
    //                     var requestTimeStamp = authorizationHeaderArray["timestamp"];

    //                     var isValid = Task.Run(() => IsValidRequest(req, clientId, mac, nonce, requestTimeStamp, bodyhash));

    //                     if (isValid.Result)
    //                     {
    //                         var user = CommonUtility.GetAPIAccessesByAPPId(clientId);
    //                         if (user == null && user.OrganisationId == null)
    //                         {
    //                             Logger.CustomLogger.LogMessage("AuthenticateAsync line 68");
    //                             context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
    //                         }
    //                         context.Principal = new GenericPrincipal(new GenericIdentity(user.OrganisationId.Value.ToString()), null);
    //                     }
    //                     else
    //                     {
    //                         Logger.CustomLogger.LogMessage("AuthenticateAsync line 78");
    //                         context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
    //                     }
    //                 }
    //                 else
    //                 {
    //                     Logger.CustomLogger.LogMessage("AuthenticateAsync line 85");
    //                     context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
    //                 }
    //             }
    //             else
    //             {
    //                 Logger.CustomLogger.LogMessage("AuthenticateAsync line 91");
    //                 context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
    //             }
    //         }

    //         return Task.FromResult(0);
    //     }

    //     public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
    //     {
    //         context.Result = new ResultWithChallenge(context.Result);
    //         return Task.FromResult(0);
    //     }

    //     public bool AllowMultiple
    //     {
    //         get { return false; }
    //     }
    //     private bool IsHAMCCheckon(HttpAuthenticationContext context)
    //     {
    //         try
    //         {
    //             var contentType = context.Request.Content.Headers.ContentType.ToString();
    //             string requestBody;
    //             using (var stream = new MemoryStream())
    //             {
    //                 var context1 = (HttpContextBase)context.Request.Properties["MS_HttpContext"];
    //                 context1.Request.InputStream.Seek(0, SeekOrigin.Begin);
    //                 context1.Request.InputStream.CopyTo(stream);
    //                 requestBody = Encoding.UTF8.GetString(stream.ToArray());
    //             }

    //             int organisationId = 0;
    //             switch (contentType)
    //             {
    //                 case "application/json; charset=utf-8":
    //                     dynamic data1 = Newtonsoft.Json.JsonConvert.DeserializeObject(requestBody);
    //                     if (data1.OrganizationKey != null)
    //                     {
    //                         organisationId = Convert.ToInt32(Cryptography.DecryptText(data1.OrganizationKey.ToString()));
    //                     }
    //                     if (data1.OrganisationKey != null)
    //                     { organisationId = Convert.ToInt32(Cryptography.DecryptText(data1.OrganizationKey.ToString())); }
    //                     break;
    //                 case "application/json":
    //                     dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(requestBody);
    //                     if (data.OrganizationKey != null)
    //                     {
    //                         organisationId = Convert.ToInt32(Cryptography.DecryptText(data.OrganizationKey.ToString()));
    //                     }
    //                     if (data.OrganisationKey != null)
    //                     { organisationId = Convert.ToInt32(Cryptography.DecryptText(data.OrganizationKey.ToString())); }
    //                     break;
    //                 case "application/x-www-form-urlencoded; charset=UTF-8":
    //                     var array = requestBody.Split('&').ToList();
    //                     var organizationKey = array.Where(c => c.IndexOf("OrganizationKey", StringComparison.OrdinalIgnoreCase) >= 0 || c.IndexOf("OrganisationKey", StringComparison.OrdinalIgnoreCase) >= 0).FirstOrDefault();
    //                     if (organizationKey != null)
    //                     {
    //                         organisationId = Convert.ToInt32(Cryptography.DecryptText(HttpUtility.UrlDecode(organizationKey.Substring(16, organizationKey.Length - 16))));
    //                     }
    //                     break;
    //             }
    //             return CommonUtility.allowedHMACOrgs.Where(x => x.OrganisationId == organisationId).Any();

    //         }
    //         catch
    //         {
    //             //  ignored
    //         }

    //         return false;
    //     }

    //     // / <summary>
    //     // / IsValidRequest
    //     // / </summary>
    //     // / <param name="request"></param>
    //     // / <param name="clientId"></param>
    //     // / <param name="mac"></param>
    //     // / <param name="nonce"></param>
    //     // / <param name="requestTimeStamp"></param>
    //     // / <param name="bodyHash"></param>
    //     // / <returns></returns>
    //     private async Task<bool> IsValidRequest(HttpRequestMessage request, string clientId, string mac, string nonce, string requestTimeStamp, string bodyHash)
    //     {
    //         if (!CommonUtility.allowedHMACOrgs.Any(x => x.AppId == clientId))
    //         {
    //             return false;
    //         }
    //         if (IsReplayRequest(nonce, requestTimeStamp))
    //         {
    //             return false;
    //         }
    //         string requestContentBase64StringOrBodyHash = string.Empty;
    //         string requestUri = HttpUtility.UrlEncode(request.RequestUri.AbsoluteUri.ToLower());
    //         var sharedKey = CommonUtility.allowedHMACOrgs.FirstOrDefault(x => x.AppId == clientId).AppKey;
    //         var secretKeyBytes = Convert.FromBase64String(sharedKey);
    //         if (request.Content != null)
    //         {
    //             // calculating bodyhash               
    //             var requestByte = await request.Content.ReadAsByteArrayAsync();
    //             if (requestByte.Length > 0)
    //             {
    //                 byte[] hash = CommonHelper.ComputeHash(requestByte, secretKeyBytes);
    //                 if (hash != null)
    //                 {
    //                     requestContentBase64StringOrBodyHash = Convert.ToBase64String(hash);
    //                     // validate body hash here
    //                     if (!requestContentBase64StringOrBodyHash.Equals(bodyHash))
    //                     {
    //                         return false;
    //                     }
    //                 }
    //             }
    //         }
    //         var signatureRawDataBytes = Encoding.UTF8.GetBytes($"{clientId}{request.Method.Method}{requestUri}{requestTimeStamp}{nonce}{requestContentBase64StringOrBodyHash}");
    //         var computedMac = Convert.ToBase64String(CommonHelper.ComputeHash(signatureRawDataBytes, secretKeyBytes));
    //         return mac.Equals(computedMac, StringComparison.Ordinal);
    //     }
    //     // / <summary>
    //     // / IsReplayRequest
    //     // / </summary>
    //     // / <param name="nonce"></param>
    //     // / <param name="requestTimeStamp"></param>
    //     // / <returns></returns>
    //     private bool IsReplayRequest(string nonce, string requestTimeStamp)
    //     {
    //         UInt16 hmacRequestMaxAgeInSeconds = Convert.ToUInt16(SiteSettings.HMACRequestMaxAgeInSeconds);
    //         AmazonDynamoDBClient amazonDynamoDbClient = new AmazonDynamoDBClient(SiteSettings.AWSAccessKeyId, SiteSettings.AWSSecretKey, RegionEndpoint.EUWest1);

    //         //  Get item from Cust_UAT_HMAC_Nonce amazon DynamoDB table
    //         Dictionary<string, AttributeValue> hmacNonceState = amazonDynamoDbClient.GetItem(new GetItemRequest
    //         {
    //             TableName = Convert.ToString(SiteSettings.HMACNonceTableName),
    //             Key = new Dictionary<string, AttributeValue>
    //             {
    //                 {
    //                    "Nonce", new AttributeValue { S = nonce }
    //                 }
    //             }
    //         }).Item;
    //         //  if nonce found
    //         if (hmacNonceState.Count > 0)
    //         {
    //             return true;
    //         }

    //         DateTime epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
    //         TimeSpan currentTs = DateTime.UtcNow - epochStart;
    //         var requestTotalSeconds = Convert.ToUInt64(requestTimeStamp);
    //         var diff = Math.Abs((decimal)(currentTs.TotalSeconds - requestTotalSeconds));
    //         if (diff > hmacRequestMaxAgeInSeconds)
    //         {
    //             return true;
    //         }

    //         Table hmacNonceTable = Table.LoadTable(amazonDynamoDbClient, SiteSettings.HMACNonceTableName);
    //         Document hmacNonceDocument = new Document
    //         {
    //             ["Nonce"] = nonce,
    //             ["Expires"] = DateTime.UtcNow.AddSeconds(hmacRequestMaxAgeInSeconds + 180),
    //             ["CreateDate"] = requestTimeStamp
    //         };
    //         hmacNonceTable.PutItem(hmacNonceDocument);
    //         return false;
    //     }
    // }
    // / <summary>
    // / 
    // / </summary>
    // public class ResultWithChallenge : IHttpActionResult
    // {
    //     private readonly IHttpActionResult next;
    //     // / <summary>
    //     // / 
    //     // / </summary>
    //     // / <param name="next"></param>
    //     public ResultWithChallenge(IHttpActionResult next)
    //     {
    //         this.next = next;
    //     }
    //     // / <summary>
    //     // / 
    //     // / </summary>
    //     // / <param name="cancellationToken"></param>
    //     // / <returns></returns>
    //     public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
    //     {
    //         var response = await next.ExecuteAsync(cancellationToken);

    //         if (response.StatusCode == HttpStatusCode.Unauthorized)
    //         {
    //             response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue(SiteSettings.HMACAuthenticationScheme));
    //         }
    //         if (response.StatusCode == HttpStatusCode.OK)
    //         {
    //         }
    //         return response;
    //     }
    // }
}

