using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Project.Core.Enum;
using Project.Models.CommonModel;
using Project.Models.Subscription;
using Project.Services.IService;
using Project.WebAPI.Models;
using System;
using System.IO;

namespace Project.WebAPI.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly Microsoft.Extensions.Configuration.IConfiguration configuataion;

        public WebhookController(IAccountService accountService, ISubscriptionService subscriptionService)
        {
            _accountService = accountService;
            _subscriptionService = subscriptionService;
            configuataion = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json").Build();
        }

        [HttpPost("CustomerCreate")]
        public IActionResult CustomerCreate(CustomerRoot response)
        {
            //do with the customer data
            //Console.Write("shopify response: " + responsestring);
            var responsestring = JsonConvert.SerializeObject(response);
            var serviceResponse = _accountService.RegisterCustomer(new RegisterViewModel()
            {
                ShopifyId = response.customer.id,
                FirstName = response.customer.first_name,
                LastName = response.customer.last_name,
                Email = response.customer.email,
                Username = response.customer.email,
                Password = "Admin@123",
                ShopifyResponse = responsestring,
                UserType = EnumUserType.User // default user or shopify
            }).GetAwaiter().GetResult();
            //send email to reset password
            _accountService.CreatePasswordLink(response.customer.email, configuataion["CustomKeys:BaseUrl"]).GetAwaiter().GetResult();
            return new OkObjectResult(response);
        }

        [HttpPost("SubscriptionCreate")]
        public IActionResult SubscriptionCreate(SubscriptionRoot response)
        {
            var serviceResponse = _subscriptionService.SaveSubscription(new SubscriptionViewModel()
            {
                SubscriptionId = response.subscription.id,
                CancellationReason = response.subscription.cancellation_reason,
                CancellationReasonComments = response.subscription.cancellation_reason_comments,
                CancelledOn = response.subscription.cancelled_at,
                //ChargeInvervalFrequency = string.IsNullOrEmpty(response.subscription.charge_interval_frequency)?0:Convert.ToInt32(response.subscription.charge_interval_frequency),
                CreatedOn = response.subscription.created_at,
                CustomerId = response.subscription.customer_id,
                NextChargeScheduleOn = response.subscription.next_charge_scheduled_at,
                //OrderInvervalFrequency = string.IsNullOrEmpty(response.subscription.order_interval_frequency)?0:Convert.ToInt32(response.subscription.order_interval_frequency),
                OrderInvervalUnit = response.subscription.order_interval_unit,
                Price = Convert.ToDecimal(response.subscription.price),
                ProductTitle = response.subscription.product_title,
                Quantity = response.subscription.quantity,
                Status = response.subscription.status,
                UpdatedOn = response.subscription.updated_at,
                VariantTitle = response.subscription.variant_title
            }).GetAwaiter().GetResult();
            
            return new OkObjectResult(response);
        }

        [HttpPost("SubscriptionActive")]
        public IActionResult SubscriptionActive(SubscriptionRoot response)
        {
            var serviceResponse = _subscriptionService.ActivateSubscription(new SubscriptionViewModel()
            {
                SubscriptionId = response.subscription.id,
                CancellationReason = null,
                CancellationReasonComments = null,
                CancelledOn = null,
                NextChargeScheduleOn = response.subscription.next_charge_scheduled_at,
                Status = response.subscription.status,
                UpdatedOn = response.subscription.updated_at
            }).GetAwaiter().GetResult();

            return new OkObjectResult(response);
        }

        [HttpPost("SubscriptionCancelled")]
        public IActionResult SubscriptionCancelled(SubscriptionRoot response)
        {
            var serviceResponse = _subscriptionService.CancelSubscription(new SubscriptionViewModel()
            {
                SubscriptionId = response.subscription.id,
                CancellationReason = response.subscription.cancellation_reason,
                CancellationReasonComments = response.subscription.cancellation_reason_comments,
                CancelledOn = response.subscription.cancelled_at,
                Status = response.subscription.status,
                NextChargeScheduleOn = response.subscription.next_charge_scheduled_at,
                UpdatedOn = response.subscription.updated_at
            }).GetAwaiter().GetResult();

            return new OkObjectResult(response);
        }

        [HttpPost("SubscriptionSkipped")]
        public IActionResult SubscriptionSkipped(SubscriptionRoot response)
        {
            var serviceResponse = _subscriptionService.SkippedSubscription(new SubscriptionViewModel()
            {
                SubscriptionId = response.subscription.id,
                NextChargeScheduleOn = response.subscription.next_charge_scheduled_at,
                UpdatedOn = response.subscription.updated_at,
                Status = response.subscription.status,
            }).GetAwaiter().GetResult();

            return new OkObjectResult(response);
        }

        [HttpPost("SubscriptionUnSkipped")]
        public IActionResult SubscriptionUnSkipped(SubscriptionRoot response)
        {
            var serviceResponse = _subscriptionService.UnSkippedSubscription(new SubscriptionViewModel()
            {
                SubscriptionId = response.subscription.id,
                NextChargeScheduleOn = response.subscription.next_charge_scheduled_at,
                UpdatedOn = response.subscription.updated_at,
                Status = response.subscription.status
            }).GetAwaiter().GetResult();

            return new OkObjectResult(response);
        }

        [HttpPost("SubscriptionUpdated")]
        public IActionResult SubscriptionUpdated(SubscriptionRoot response)
        {
            var serviceResponse = _subscriptionService.UnSkippedSubscription(new SubscriptionViewModel()
            {
                SubscriptionId = response.subscription.id,
                NextChargeScheduleOn = response.subscription.next_charge_scheduled_at,
                UpdatedOn = response.subscription.updated_at,
                Status = response.subscription.status
            }).GetAwaiter().GetResult();

            return new OkObjectResult(response);
        }
    }
}
