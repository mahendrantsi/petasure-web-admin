using Project.Core.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.Pets
{
    public class PetsViewModel
    {
        [Required(ErrorMessage = "Please enter Pet Name")]
        [Display(Name = "Pet Name")]
       // [MaxLength(20, ErrorMessage = "First Name should not exceed 20 characters")]
        [RegularExpression(Setting.WhiteSpaceRegex, ErrorMessage = "Please enter valid Pet Name")]
        public string PName { get; set; }

        public string PSex { get; set; }


        [Required(ErrorMessage = "Please enter Address")]
        [Display(Name = "Address")]
        // [MaxLength(20, ErrorMessage = "First Name should not exceed 20 characters")]
        [RegularExpression(Setting.WhiteSpaceRegex, ErrorMessage = "Please enter valid Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please enter contact number")]
        [Display(Name = "Contact Number")]
        [RegularExpression("^(\\d{10,11})$", ErrorMessage = "Please enter valid contact number")]
        public string ContactNumber { get; set; }


       
        [Display(Name = "Owner Id")]
        // [MaxLength(20, ErrorMessage = "First Name should not exceed 20 characters")]
        //[RegularExpression(Setting.WhiteSpaceRegex, ErrorMessage = "Please enter valid Pet Name")]
        public Guid PetOwnerId { get; set; }


        public Guid Id { get; set; }

        public int? PetTypeId { get; set; }
        public string PetTypeName { get; set; }

        public Boolean IsMissing { get; set; }
        

        public string NoseImagePath { get; set; }
        public string FullBodyImagePath { get; set; }
        public string FaceImagePath { get; set; }

        public Decimal Lat { get; set; }
        public Decimal Long { get; set; }

        public DateTime CreatedOn { get; set; }

        public string MicrochipNumber { get; set; }

        public int? PetTypeId { get; set; }

        public string? Breeder { get; set; }

        //Not Used in Pet Profile Now
        public string? LicenceNumber { get; set; }

        //Not Used in Pet Profile Now
        public string? IssuingAuthority { get; set; }
        public string? BreedDescription { get; set; }
        public string? Colour { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
