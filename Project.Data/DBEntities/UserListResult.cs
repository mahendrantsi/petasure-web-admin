using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Data.DBEntities
{
    [Keyless]
    public  class UserListResult
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string RoleName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public long RoleId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CustomerType { get; set; }
        public long CreatedBy { get; set; }
       
        
    }
}
