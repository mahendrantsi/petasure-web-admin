namespace Project.Core.Extension
{
    using Project.Core.Model;
    using System;

    public class APICoreRes : Bridge
    {
        public APICoreRes()
        {
            ResponseTime = DateTime.UtcNow;
            RequestTime = ReqDate;
        }


        public string userMessage { get; set; }
        public string suggestdRemedialAction { get; set; }
        public DateTime RequestTime { get; set; }
        public DateTime ResponseTime { get; set; }
        public int RecordsCount { get; set; }
        public ResStatus status { get; set; }
 
        public enum ResStatus
        {
            Fail = 0,
            Success = 1,
            ValidationError = 2
        }
    }
}