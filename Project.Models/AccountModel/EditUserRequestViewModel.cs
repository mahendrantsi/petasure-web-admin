using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.AccountModel
{
    public class EditUserRequestViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? UserImage { get; set; }
        public string? LicenseNumber { get; set; }
        public string? IssuingAuthority { get; set; }
    }
    
    public class UploadImageRequestViewModel
    {
        public IFormFile Image { get; set; }
        public string FolderName { get; set; }
    }
}
