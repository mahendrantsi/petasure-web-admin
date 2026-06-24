using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Data.ExtendedDBEntities;

namespace Project.Data.DBEntities
{
    public class UserPasswordToken : BaseEntity
    {
        public string Code { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserID { get; set; }
        
        // Navigation Property
        public virtual DerivedIdentityUser User { get; set; }
    }
}
