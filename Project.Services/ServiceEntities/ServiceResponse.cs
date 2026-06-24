namespace Project.Services.ServiceEntities
{
    using System;

    public class ServiceResponse<T>
        where T : class
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }
        public T Errors { get; set; }

        public DateTime StartOn { get; set; }

        public DateTime EndOn { get; set; }

        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }

        public ServiceResponse()
        {
            this.StartOn = DateTime.UtcNow;
        }
    }
}
