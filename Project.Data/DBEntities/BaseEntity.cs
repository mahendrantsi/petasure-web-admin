namespace Project.Data.DBEntities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Net.Http.Headers;
    using System.Reflection.Metadata.Ecma335;

    public class BaseEntity : IBaseEntity
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }=DateTime.UtcNow; 
        public Guid CreatedBy { get; set; }
    }
}
