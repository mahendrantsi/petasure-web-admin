using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Data.DBEntities
{
   public class DerivedIdentityRole : IdentityRole<Guid>
    {
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
