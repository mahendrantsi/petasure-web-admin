namespace Project.Data.DBEntities
{
    using System;

    public class ManageRoleBasePermission : BaseEntity
    {

        public long RoleId { get; set; }

        public long MenuId { get; set; }

        public long? UserId { get; set; }
        public bool? IsReadOnly { get; set; }

        public bool? IsReadWrite { get; set; }

        public bool? IsFullAccess { get; set; }

        public bool IsActive { get; set; }

        public DateTime ModifiedOn { get; set; } = DateTime.UtcNow;

        public long ModifiedBy { get; set; }
    }
}