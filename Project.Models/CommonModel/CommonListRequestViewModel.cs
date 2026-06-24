using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.CommonModel
{
    public class CommonListRequestViewModel
    {
        [Required]
        public long UserId { get; set; }
        public int PageSize { get; set; } = 0;
        public int PageNo { get; set; }
    }
}
