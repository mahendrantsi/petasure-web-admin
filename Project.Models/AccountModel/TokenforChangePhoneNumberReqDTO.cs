using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.AccountModel
{
    public class TokenforChangePhoneNumberReqDTO
    {
        [Required]
        public string NewPhoneNumber { get; set; }
    }
}
