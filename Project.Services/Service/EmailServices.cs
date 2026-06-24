using Project.Models.CommonModel;
using Project.Services.IService;
using Project.Services.Resources;
using Project.Services.ServiceEntities;
using Project.Services.ServiceHelper;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Project.Data.ExtendedDBEntities;
using ServiceStack.Auth;
using System.Collections.Generic;
using SendGrid.Helpers.Mail;
using SendGrid;
using AutoMapper.Configuration;
using Project.Core.Enum;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Reflection;
using Microsoft.WindowsAzure.Storage;

namespace Project.Services.Service
{
	public class EmailService : BaseService, IEmailService
	{
		private readonly EmailConfig config;
		private readonly SmtpModel smtpModel = null;
		private readonly IEmailLogService emailLogService;
		private readonly UserManager<DerivedIdentityUser> _userManager;
		private static string APIKey = "";
		private static string FromEmailAddress = "";
		private readonly FromMailViewModel fromModel = null;
		private readonly Microsoft.Extensions.Configuration.IConfiguration configuataion;
		private readonly string BaseURL = string.Empty;

		public EmailService(IOptions<EmailConfig> emailConfig, IEmailLogService objEmailLogService,
							UserManager<DerivedIdentityUser> userManager)
		{
			this.config = emailConfig.Value;
			emailLogService = objEmailLogService;
			smtpModel = new SmtpModel
			{
				SmtpUser = config.SmtpUser,
				SmtpHost = config.SmtpHost,
				SmtpPassword = config.SmtpPassword,
				SmtpPort = config.SmtpPort,
				EnableSsl = config.EnableSsl,
			};
			_userManager = userManager;

			configuataion = new ConfigurationBuilder()
				   .SetBasePath(Directory.GetCurrentDirectory())
				   .AddJsonFile("appsettings.json").Build();
			BaseURL = configuataion["CustomKeys:BaseUrl"];
		}

		public async Task SendLinkForgotAsync(string userName, string ResetPasswordLink)
		{
			var user = await _userManager.FindByEmailAsync(userName);
			string mailBodyContent = (new StreamReader(Path.Combine("Templates/ForgotPwdEmail.html"))).ReadToEnd();

			mailBodyContent = mailBodyContent.Replace("{ResetPasswordLink}", ResetPasswordLink);
			mailBodyContent = mailBodyContent.Replace("{username}", user.UserName);
			mailBodyContent = mailBodyContent.Replace("{fullname}", user.FirstName + " " + user.LastName);

			MailViewModel mailViewModel = new MailViewModel
			{
				MailTo = userName,
				Title = "Password Reset Request",
				Content = mailBodyContent,
			};
			await EmailHelper.Send(mailViewModel, smtpModel);
		}

		public async Task SendLinkCreateAsync(string userName, string ResetPasswordLink)
		{
			var user = await _userManager.FindByEmailAsync(userName);
			string mailBodyContent = (new StreamReader(Path.Combine("Templates/CreatePwdEmail.html"))).ReadToEnd();

			mailBodyContent = mailBodyContent.Replace("{ResetPasswordLink}", ResetPasswordLink);
			mailBodyContent = mailBodyContent.Replace("{fullname}", user.FirstName + " " + user.LastName);

			MailViewModel mailViewModel = new MailViewModel
			{
				MailTo = userName,
				Title = "Password Create Request",
				Content = mailBodyContent,
			};
			await EmailHelper.Send(mailViewModel, smtpModel);
		}


		public async Task SendSecondaryUserEmailAsync(string emailID, string passwordString)
		{
			var user = await _userManager.FindByEmailAsync(emailID);
			string mailBodyContent = (new StreamReader(Path.Combine("Templates/SecondaryUserLogin.html"))).ReadToEnd();

			mailBodyContent = mailBodyContent.Replace("{fullname}", user.FirstName + " " + user.LastName);
			mailBodyContent = mailBodyContent.Replace("{username}", emailID);
			mailBodyContent = mailBodyContent.Replace("{password}", passwordString);

			MailViewModel mailViewModel = new MailViewModel
			{
				MailTo = emailID,
				Title = "Login Details",
				Content = mailBodyContent,
			};
			await EmailHelper.Send(mailViewModel, smtpModel);
		}

		public async Task SendMissingPetAcknowledgeEmail(string emailID)
		{
			var user = await _userManager.FindByEmailAsync(emailID);
			string mailBodyContent = (new StreamReader(Path.Combine("Templates/MissingPetAcknowledge.html"))).ReadToEnd();

			mailBodyContent = mailBodyContent.Replace("{fullname}", user.FirstName + " " + user.LastName);

			MailViewModel mailViewModel = new MailViewModel
			{
				MailTo = emailID,
				Title = "Missing Pet Acknowledgement",
				Content = mailBodyContent,
			};
			await EmailHelper.Send(mailViewModel, smtpModel);
		}

		public async Task SendMissingPetSupportEmail(string emailID, string petname)
		{
			string mailBodyContent = (new StreamReader(Path.Combine("Templates/MissingPetSupport.html"))).ReadToEnd();

			mailBodyContent = mailBodyContent.Replace("{username}", emailID);
			mailBodyContent = mailBodyContent.Replace("{petname}", petname);

			MailViewModel mailViewModel = new MailViewModel
			{
				MailTo = configuataion["CustomKeys:PetaSupportEmail"],//"missing.alert@petasure.co.uk",
				Title = "Missing Pet",
				Content = mailBodyContent,
			};
			await EmailHelper.Send(mailViewModel, smtpModel);
		}

		public async Task SendFoundMissingPetSupportEmail(string emailID, string petname, string phone)
		{
			string mailBodyContent = (new StreamReader(Path.Combine("Templates/FoundMissingPetSupport.html"))).ReadToEnd();

			mailBodyContent = mailBodyContent.Replace("{petname}", petname);
			mailBodyContent = mailBodyContent.Replace("{phone}", phone);
			mailBodyContent = mailBodyContent.Replace("{email}", emailID);


			MailViewModel mailViewModel = new MailViewModel
			{
				MailTo = configuataion["CustomKeys:PetaSupportEmail"],//"missing.alert@petasure.co.uk",
				Title = "Missing Pet",
				Content = mailBodyContent,
			};
			await EmailHelper.Send(mailViewModel, smtpModel);
		}


		private async Task<MailViewModel> CreateLog(MailViewModel mailViewModel)
		{
			EmailLogViewModel emailLogViewModel = new EmailLogViewModel
			{
				FromMail = config.SmtpUser,
				ToMail = mailViewModel.MailTo,
				CcMail = mailViewModel.CC,
				BccMail = mailViewModel.BCC,
				Subject = mailViewModel.Title,
				CreatedOn = DateTime.UtcNow,
				MailStatus = "This mail is Successfully send",
			};
			await emailLogService.Create(emailLogViewModel);
			return mailViewModel;
		}

		public static async Task<string> SendMail(string subject, string toEmail, string plainContent, string htmlContent, byte[] byteData = null, string filename = "", SendGrid.Helpers.Mail.Attachment calendarAttachment = null)
		{
			try
			{
				var client = new SendGridClient(APIKey);
				var from = new EmailAddress(FromEmailAddress, "Hone");
				var to = new EmailAddress(toEmail);
				var msg = MailHelper.CreateSingleEmail(from, to, subject, plainContent, htmlContent);
				if (calendarAttachment != null)
				{
					msg.Attachments = new List<SendGrid.Helpers.Mail.Attachment>() { calendarAttachment };
				}
				if (byteData != null)
				{
					var baseString = Convert.ToBase64String(byteData);
					if (filename.Contains("pdf"))
						msg.AddAttachment(filename, baseString, type: "application/pdf");
					if (filename.Contains("csv"))
						msg.AddAttachment(filename, baseString, type: "text/csv");
				}

				var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
				return response.StatusCode.ToString();
			}

			catch (Exception ex)
			{
				return "Failed";
			}
		}
	}
}
