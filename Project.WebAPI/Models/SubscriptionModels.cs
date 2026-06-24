using System.Collections.Generic;
using System;

namespace Project.WebAPI.Models
{   

    public class SubscriptionRoot
    {
        public Subscription subscription { get; set; }
    }

    public class Subscription
    {
        public int address_id { get; set; }
        public AnalyticsData analytics_data { get; set; }
        public string cancellation_reason { get; set; }
        public string cancellation_reason_comments { get; set; }
        public DateTime? cancelled_at { get; set; }
        public string charge_delay { get; set; }
        public string charge_interval_frequency { get; set; }
        public DateTime created_at { get; set; }
        public int customer_id { get; set; }
        public string cutoff_day_of_month_before_and_after { get; set; }
        public string cutoff_day_of_week_before_and_after { get; set; }
        public string email { get; set; }
        public string expire_after_specific_number_of_charges { get; set; }
        public string first_charge_date { get; set; }
        public int has_queued_charges { get; set; }
        public int id { get; set; }
        
        public string locked_pending_charge_id { get; set; }
        public int max_retries_reached { get; set; }
        public DateTime? next_charge_scheduled_at { get; set; }
        public string order_day_of_month { get; set; }
        public string order_day_of_week { get; set; }
        public string order_interval_frequency { get; set; }
        public string order_interval_unit { get; set; }
        public string presentment_currency { get; set; }
        public double price { get; set; }
        public string product_title { get; set; }
        public List<Property_2021_11> properties { get; set; }
        public int quantity { get; set; }
        public int recharge_product_id { get; set; }
        public long shopify_product_id { get; set; }
        public long shopify_variant_id { get; set; }
        public string sku { get; set; }
        public bool sku_override { get; set; }
        public string status { get; set; }
        public DateTime updated_at { get; set; }
        public string variant_title { get; set; }
    }

    
    public class AnalyticsData_2021_11
    {
        public List<object> utm_params { get; set; }
    }

    public class ExternalProductId_2021_11
    {
        public string ecommerce { get; set; }
    }

    public class ExternalVariantId_2021_11
    {
        public string ecommerce { get; set; }
    }

    public class Property_2021_11
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class SubscriptionRoot_2021_11
    {
        public Subscription_2021_11 subscription { get; set; }
    }

    public class Subscription_2021_11
    {
        public int id { get; set; }
        public int address_id { get; set; }
        public int customer_id { get; set; }
        public AnalyticsData_2021_11 analytics_data { get; set; }
        public string cancellation_reason { get; set; }
        public string cancellation_reason_comments { get; set; }
        public DateTime? cancelled_at { get; set; }
        public int charge_interval_frequency { get; set; }
        public DateTime created_at { get; set; }
        public string expire_after_specific_number_of_charges { get; set; }
        public ExternalProductId_2021_11 external_product_id { get; set; }
        public ExternalVariantId_2021_11 external_variant_id { get; set; }
        public bool has_queued_charges { get; set; }
        public bool is_prepaid { get; set; }
        public bool is_skippable { get; set; }
        public bool is_swappable { get; set; }
        public bool max_retries_reached { get; set; }
        public DateTime? next_charge_scheduled_at { get; set; }
        public string order_day_of_month { get; set; }
        public string order_day_of_week { get; set; }
        public int order_interval_frequency { get; set; }
        public string order_interval_unit { get; set; }
        public string presentment_currency { get; set; }
        public string price { get; set; }
        public string product_title { get; set; }
        public List<Property_2021_11> properties { get; set; }
        public int quantity { get; set; }
        public string sku { get; set; }
        public bool sku_override { get; set; }
        public string status { get; set; }
        public DateTime updated_at { get; set; }
        public string variant_title { get; set; }
    }


}
