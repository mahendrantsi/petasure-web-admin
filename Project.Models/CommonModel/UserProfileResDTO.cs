using Project.Models.ProfileModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.CommonModel
{
    public class UserProfileResDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string BusinessName { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public long UserID { get; set; }

    }
}
