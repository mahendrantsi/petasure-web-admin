using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Project.Core.Extension;
using Project.Core.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Project.Core.NotificationProvider
{
    public class CommonProvider
    {
        private readonly IConfiguration configuration; 

        public CommonProvider(IConfiguration iConfig)
        {
            configuration = iConfig;
        }

        public string GetCustomTopic()
        {
            DateTime dtNow = DateTime.Now;
            return "tp" + dtNow.Date.Day.ToString() + dtNow.Date.Month.ToString() + dtNow.Date.Year.ToString() +
                dtNow.Hour.ToString() + dtNow.Minute.ToString() + dtNow.Second.ToString() + dtNow.Millisecond.ToString() + dtNow.Ticks.ToString();
        }

        public string TopicSubscription(string topic, List<string> lstTokens, bool isSubscribe = true)
        {
            string serverKey = configuration.GetSection("MySettings").GetSection("NotificationServerKey").Value;
            string urlSubscribeTopic = configuration.GetSection("MySettings").GetSection("SubscribeTopicUrl").Value;
            string urlUnSubscribeTopic = configuration.GetSection("MySettings").GetSection("UnSubscribeTopicUrl").Value;

            string response;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                WebRequest tRequest = WebRequest.Create(isSubscribe ? urlSubscribeTopic : urlUnSubscribeTopic);
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data = new
                {
                    to = "/topics/" + topic,
                    registration_tokens = lstTokens
                };

                var json = JsonConvert.SerializeObject(data);

                byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
                tRequest.ContentLength = byteArray.Length;

                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                string sResponseFromServer = tReader.ReadToEnd();
                                response = sResponseFromServer; 
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response = ex.Message;
            }
            return response;
        }

        public string SendNotificationToTopic(string topic, NotificationModel notificationModel)
        {
            string serverKey = configuration.GetSection("MySettings").GetSection("NotificationServerKey").Value;
            string senderId = configuration.GetSection("MySettings").GetSection("NotificationServerId").Value;
            string urlSendNotification = configuration.GetSection("MySettings").GetSection("SendNotificationUrl").Value;

            string response;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                WebRequest tRequest = WebRequest.Create(urlSendNotification);
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var message = StripHTML(notificationModel.Message);
                var imageUrl = string.Empty;
       

                var dataList = new NotificationDataModel
                {
                    to = topic,
                    notification = new Notifications
                    {
                        body = message,
                        title = notificationModel.Sub,
                        badge = null,
                        sound = null,
                        mutable_content = true,
                        content_available = true,
                        mediaType = "image",
                        mediaUrl = imageUrl,
                        image = imageUrl,
                        category = notificationModel.NotificationCategory,
                        click_action = "android.intent.action.MAIN"
                    },
                    data = new Datas
                    {
                        category = notificationModel.NotificationCategory,
                        notificationId = notificationModel.Id,
                        eventId = Convert.ToInt32(notificationModel.NotificationCategoryId)
                    }
                };

                var json = JsonConvert.SerializeObject(dataList);
                byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                tRequest.ContentLength = byteArray.Length;

                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                string sResponseFromServer = tReader.ReadToEnd();
                                response = sResponseFromServer;
                                 
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { 
                response = ex.Message;
            }
            return response;
        }

        public static string StripHTML(string input)
        {
            Regex rx = new Regex("<[^>]*>", RegexOptions.None, TimeSpan.FromMilliseconds(100));
            var strHtml = rx.Replace(input, "");
            strHtml = strHtml.Replace("\n", " ");
            strHtml = strHtml.Replace("&nbsp;", " ");
            strHtml = strHtml.Replace("&amp;", "& ");
            strHtml = strHtml.Replace("&#39;", "'");
            strHtml = strHtml.Replace("&sbquo;", ",");
            strHtml = strHtml.Replace("&rsquo;", "'");

            // ASCII Html
            strHtml = strHtml.Replace("&szlig;", "ß");
            strHtml = strHtml.Replace("&auml;", "ä");
            strHtml = strHtml.Replace("&ouml;", "ö");
            strHtml = strHtml.Replace("&uuml;", "ü");
            strHtml = strHtml.Replace("&Auml;", "Ä");
            strHtml = strHtml.Replace("&Ouml;", "Ö");
            strHtml = strHtml.Replace("&Uuml;", "Ü");
            strHtml = strHtml.Replace("&Eacute;", "É");
            strHtml = strHtml.Replace("&eacute;", "é");
            strHtml = strHtml.Replace("&easter;", "⩮");

            // Other characters
            strHtml = strHtml.Replace("&pound;", "£");
            return strHtml;
        }

    }
}
