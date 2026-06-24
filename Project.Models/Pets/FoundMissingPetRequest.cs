using Project.Data.DBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.Pets
{
    public  class FoundMissingPetRequest
    {
        public Guid PetId { get; set; }

        public PetStatus? Status { get; set; }

        public Guid? FoundBy { get; set; }


        public string? Address { get; set; }

        public Decimal? Lat { get; set; }
        public Decimal? Long { get; set; }

        public string? Email { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }
        public string? ContactNumber { get; set; }

    }
}
