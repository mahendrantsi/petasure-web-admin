
namespace Project.Models.AdminModel
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Project.Core.Enum;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class RoleUserViewModel
    {
        public long Id { get; set; }

        public string Enc_Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string NormalizedName { get; set; }
    }

    public class UserViewModel
    {
        public Guid Id { get; set; }

        public string Enc_Id { get; set; }

        public long? RoleId { get; set; }

        public string FirstName { get; set; }
        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string RoleName { get; set; }

        public string LastName { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedOn { get; set; }

        public string StrCreatedOn { get; set; }
     

        public string UserImage { get; set; }

        public SelectList Roles { get; set; }
    }

}
