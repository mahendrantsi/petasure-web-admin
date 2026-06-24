using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.DBEntities
{
    public class Settings : BaseEntity
    { 
        public bool IsEmailConfirmed { get; set; }
        public bool IsPhoneVerificationRequired{ get; set; }
    }
}
