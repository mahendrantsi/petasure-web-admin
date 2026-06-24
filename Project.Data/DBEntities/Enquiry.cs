using Project.Core.Enum;
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
    public class Enquiry : BaseEntity
    {
        [Required]
        public string FullName { get; set; }
        public string PhoneNo { get; set; }
        [Required]
        public string Email { get; set; } 
        [Required]
        public string Message { get; set; }
        public string Subject { get; set; }

        public int? ReadBy { get; set; }
        public EnumEnquiryType EnquiryType { get; set; } = EnumEnquiryType.Enquiry;
        
        [ForeignKey(nameof(User))]
        public Guid? UserId{ get; set; }

        public Nullable<DateTime> ReadOn { get; set; }
        public EnumEnquiryStatus Status { get; set; }

        [Required]
        public Guid EnquiryCode { get; set; } = Guid.NewGuid();

        // Navigation Property
        public virtual DerivedIdentityUser User { get; set; }
    }

    public class EnquiryResponse : BaseEntity
    {       
        [ForeignKey(nameof(Enquiry))]
        [Required]
        public Guid EnquiryID { get; set; }
        public string Answer { get; set; }
        public string PlainAnswer { get; set; }
        public bool SendMail { get; set; }
        public EnumEnquiryStatus Status { get; set; } 
        
    }
}
