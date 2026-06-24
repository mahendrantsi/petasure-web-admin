using Project.Core.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.DBEntities
{
    public class Favorites
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long UserID { get; set; }
        public long FavoriteUserID { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
