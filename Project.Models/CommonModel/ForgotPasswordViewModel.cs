using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.CommonModel
{
   public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Valid email address is required.")]
        [RegularExpression("^[A-Za-z0-9_\\+-]+(\\.[A-Za-z0-9_\\+-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*\\.([A-Za-z]{2,4})$", ErrorMessage = "Email id must be  valid .")]
        public string EMail { get; set; }

        //[Required]
        //public string UserType { get; set; }
    }
}
