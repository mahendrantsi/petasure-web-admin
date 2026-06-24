using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.DBEntities
{
    public class MissingPetsLogs : BaseEntity
    {
        [ForeignKey(nameof(MissingPet))]
        public Guid? MissingPetsID { get; set; }
        
        [ForeignKey(nameof(Pet))]
        public Guid? PetId { get; set; }

        [MaxLength(500)]
        public string Address { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public Decimal Lat { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public Decimal Long { get; set; }
        public DateTime LostDate { get; set; }



        [MaxLength(500)]
        public string FoundAddress { get; set; }

        [Column(TypeName = "decimal(18,10)")]
        public Decimal? FoundLat { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public Decimal? FoundLong { get; set; }

        public Guid? FoundBy { get; set; }
        public PetStatus Status { get; set; } = PetStatus.Lost;

        //Added New Coloum 
        public string MicrochipNumber { get; set; }

        // Navigation Properties
        public virtual MissingPets MissingPet { get; set; }
        public virtual PetInfo Pet { get; set; }
    }
}
