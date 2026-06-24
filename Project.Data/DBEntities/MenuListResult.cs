namespace Project.Data.DBEntities
{
    using Microsoft.EntityFrameworkCore;
    using System;

    [Keyless]
    public class MenuListResult
    {
        public long Id { get; set; }
        public string MenuName { get; set; }
        public int ParentId { get; set; }
        public string DisplayName { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Url { get; set; }
        public bool? IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public string Icon { get; set; }
        public bool IsDefault { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long CreatedBy { get; set; }
    }
}
