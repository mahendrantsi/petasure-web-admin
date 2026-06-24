namespace Project.Core.Extension
{
    using System;

    /// <summary>
    /// Response data.
    /// </summary>
    public class ResponseData : Bridge
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseData"/> class.
        /// </summary>
        public ResponseData()
        {
            coreRes = new APICoreRes();
            ReqDate = DateTime.UtcNow;
        }

        public APICoreRes coreRes { get; set; }
        public dynamic Data { get; set; }
        public void SetCoreResponse(dynamic data, APICoreRes.ResStatus status, string message)
        {
            coreRes.status = status;
            coreRes.userMessage = message;
            coreRes.ResponseTime = DateTime.UtcNow;
            Data = data;
        }
    }
}