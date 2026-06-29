using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Data.ExtendedDBEntities;

namespace Project.Data.DBEntities
{
    public class PetInfo : BaseEntity
    {

        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string PName { get; set; }
        
        public string PSex { get; set; }

        public string Address { get; set; }

        [ProtectedPersonalData]
        public string ContactNumber { get; set; }

        [ForeignKey(nameof(Owner))]
        public Guid? UserID { get; set; }

        public bool IsMissing { get; set; }

        public string PDataScienceId { get; set; } 
        public bool IsDelete { get; set; }
        [ForeignKey(nameof(PetTypeMaster))]
        public int? PetTypeId { get; set; }

        public string NoseImagePath { get; set; }
        public string FullBodyImagePath { get; set; }
        public string FaceImagePath { get; set; }

        public virtual PetTypeMaster PetTypeMaster { get; set; }

        [Column(TypeName = "decimal(18,10)")]
        public Decimal Lat { get; set; }
        
        [Column(TypeName = "decimal(18,10)")]
        public Decimal Long { get; set; }

        public string MicrochipNumber { get; set; }

        public string Breeder { get; set; }

        public string LicenceNumber { get; set; }

        public string IssuingAuthority { get; set; }
        public string BreedDescription { get; set; }
        public string Colour { get; set; }
        public DateTime DateOfBirth { get; set; } 

        // Navigation Properties
        public virtual DerivedIdentityUser Owner { get; set; }
        public virtual ICollection<MissingPets> MissingPets { get; set; } = new List<MissingPets>();
        public virtual ICollection<MissingPetsLogs> MissingPetsLogs { get; set; } = new List<MissingPetsLogs>();
    }
}
