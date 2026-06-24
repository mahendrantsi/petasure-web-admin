using Project.Core.Enum;
using Project.Data.ExtendedDBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.CommonModel
{
    public class UserDetailModel : DerivedIdentityUser
    {
        public string role { get; set; }
        public string CreatedOnstr { get 
            {
               return CreatedOn.ToString("dd/MM/yyyy");
            } 
        }
        public int ShopifyId { get; set; }
        public EnumUserType UserType { get; set; }
    }
}
