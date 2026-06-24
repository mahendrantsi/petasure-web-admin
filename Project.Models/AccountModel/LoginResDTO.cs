using Project.Models.AdminModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.AccountModel
{
    public class LoginResDTO
    {
        public dynamic LoggedInUser {get; set; }
        public dynamic AuthToken { get; set; }

        public UserProfileViewModel UserDetails { get; set; }
    }
}
