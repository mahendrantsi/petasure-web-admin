using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Project.Models.AdminModel
{
   public class MenuViewModel
    {
        public long Id { get; set; }

        public string Enc_Id { get; set; }

        [Required]
        public string MenuName { get; set; }
        public int? ParentId { get; set; }

        [Required]
        public string DisplayName { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        [Required]
        public string Url { get; set; }
        public bool? IsActive { get; set; }

        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid Number")]
        [Range(1, int.MaxValue, ErrorMessage = "The field {0} must be greater than or equal {1}.")]
        public int? DisplayOrder { get; set; }
        public string Icon { get; set; }

        [Display(Name ="Is Default")]
        public bool? IsDefault { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public SelectList ParentMenus { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public List<MenuViewModel> ChildMenuList { get; set; }
        public MenuViewModel()
        {
            this.ChildMenuList = new List<MenuViewModel>();
        }
    }

    public class MenuListViewModel
    {
        public long Id { get; set; }

        public string Enc_Id { get; set; }

        public string ParentMenu { get; set; }

        public string MenuName { get; set; }

        public string DisplayName { get; set; }

        public string Url { get; set; }

        public bool? IsActive { get; set; }
    }
}
