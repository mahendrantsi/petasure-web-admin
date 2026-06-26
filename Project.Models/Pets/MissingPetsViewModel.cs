using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.Pets
{
    public  class MissingPetsViewModel
    {
        public Guid? Id { get; set; }

        public Guid PetId { get; set; }
        public int? PetTypeId { get; set; }
        public string Name { get; set; }
        public string ContactNo { get; set; }

        public string Address { get; set; }

        public Decimal Lat { get; set; }
        public Decimal Long { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public DateTime LostDate { get; set; }


        public Guid? FoundBy { get; set; }
        public Enum Status { get; set; }
        public string OwnerName { get; set; }
        public string OwnerEmail { get; set; }

        public DateTime CreatedDate { get; set; }
    }

    public class IDCheckViewModel
    {
        public Guid? MissingPetId { get; set; }

        public Guid PetId { get; set; }
        public string GuestName { get; set; }
        public string GuestContactNo { get; set; }
        public string GuestEmail { get; set; }
        
        public string PetName { get; set; }
        public string PetOwnerName { get; set; }
        public string PetOwnerContactNo { get; set; }
        public string PetOwnerEmail { get; set; }

        public string Address { get; set; }

        

        [MaxLength(500)]
        public string Description { get; set; }

        public DateTime LostDate { get; set; }

        public Guid? FoundBy { get; set; }
        
    }
}
