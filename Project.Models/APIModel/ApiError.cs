namespace Project.Models.APIModel
{
    using System.Runtime.Serialization;

    [DataContract]
    public sealed class ApiError
    {
        [DataMember(EmitDefaultValue = false)]
        public object Message { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Field { get; set; }

        public dynamic Data { get; set; }
    }
}