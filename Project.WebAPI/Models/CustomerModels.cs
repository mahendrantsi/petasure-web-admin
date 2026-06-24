using System.Collections.Generic;
using System;

namespace Project.WebAPI.Models
{
    public class CustomerRoot
    {
        public Customer customer { get; set; }
    }

    public class AnalyticsData
    {
        public IList<object> utm_params { get; set; }
    }

    public class Customer
    {
        public int accepts_marketing { get; set; }
        public AnalyticsData analytics_data { get; set; }
        public bool apply_credit_to_next_checkout_charge { get; set; }
        public bool apply_credit_to_next_recurring_charge { get; set; }
        public string billing_address1 { get; set; }
        public object billing_address2 { get; set; }
        public string billing_city { get; set; }
        public object billing_company { get; set; }
        public string billing_country { get; set; }
        public string billing_phone { get; set; }
        public string billing_province { get; set; }
        public string billing_zip { get; set; }
        public object can_add_payment_method { get; set; }
        public DateTime created_at { get; set; }
        public string email { get; set; }
        public object first_charge_processed_at { get; set; }
        public string first_name { get; set; }
        public bool has_card_error_in_dunning { get; set; }
        public bool has_valid_payment_method { get; set; }
        public string hash { get; set; }
        public int id { get; set; }
        public string last_name { get; set; }
        public int number_active_subscriptions { get; set; }
        public int number_subscriptions { get; set; }
        public bool payment_method_fallback_on_failure_disabled { get; set; }
        public object phone { get; set; }
        public string processor_type { get; set; }
        public string reason_payment_method_not_valid { get; set; }
        public string shopify_customer_id { get; set; }
        public string status { get; set; }
        public bool tax_exempt { get; set; }
        public DateTime updated_at { get; set; }
    }

    public class Example
    {
        public Customer customer { get; set; }
    }
}
