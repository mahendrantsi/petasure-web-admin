using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Data.DBEntities
{
    public class PetTypeMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string TypeName { get; set; }

        public string Description { get; set; }

        public virtual ICollection<PetInfo> Pets { get; set; } = new List<PetInfo>();
    }
}
