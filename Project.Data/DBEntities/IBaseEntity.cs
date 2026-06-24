namespace Project.Data.DBEntities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public interface IBaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid CreatedBy { get; set; }
    }
}
