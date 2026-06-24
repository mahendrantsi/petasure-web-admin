using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.DBEntities
{
    public class RegistrationOTP : BaseEntity
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        [MaxLength(4)]
        public int OTP { get; set; }
        [Required]
        public bool IsActive { get; set; } = true;
    }
}
