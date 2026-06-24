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

    public enum PetStatus
    {
        Lost,       // 0
        Found,      // 1
        IDCheck    // 2
    }
    public class MissingPets : BaseEntity
    {
        
        [ForeignKey(nameof(Pet))]
        public Guid? PetId { get; set; }

        [MaxLength(500)]
        public string Address { get; set; }
        
        [MaxLength(500)]
        public string Description { get; set; }

        [Column(TypeName = "decimal(18,10)")]
        public  Decimal Lat { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public Decimal Long { get; set; }
        public DateTime LostDate { get; set; }

        [MaxLength(500)]
        public string FoundAddress { get; set; }

        [Column(TypeName = "decimal(18,10)")]
        public Decimal? FoundLat { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public Decimal? FoundLong { get; set; }

        [ForeignKey(nameof(FoundByUser))]
        public Guid? FoundBy { get; set; }
        
        public PetStatus Status { get ; set; } = PetStatus.Lost;

        //Added New Coloum 
        public string MicrochipNumber { get; set; }
        
        // Navigation Properties
        public virtual PetInfo Pet { get; set; }
        public virtual DerivedIdentityUser FoundByUser { get; set; }
        public virtual ICollection<MissingPetsLogs> Logs { get; set; } = new List<MissingPetsLogs>();
    }

    
}
