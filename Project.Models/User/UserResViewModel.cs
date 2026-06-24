using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.User
{
    public class UserResViewModel
    {
            public string uuid { get; set; }
            public string applicationUuid { get; set; }
            public string applicationUserId { get; set; }
            public DateTime createdAt { get; set; }
            public object[] institutionConsents { get; set; }
       
    }
}
