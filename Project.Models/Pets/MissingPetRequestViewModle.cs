using Project.Data.DBEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.Pets
{
    public class MissingPetRequestViewModel
    {
        public Guid PetId { get; set; }

        public string Address { get; set; }

        public Decimal Lat { get; set; }
        public Decimal Long { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public DateTime LostDate { get; set; }


        public Guid? FoundBy { get; set; }
        public PetStatus Status { get; set; }
    }
}
