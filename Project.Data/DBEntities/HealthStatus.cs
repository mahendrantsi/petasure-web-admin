using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Data.DBEntities
{
    /// <summary>
    /// A single detected condition / finding belonging to a <see cref="HealthCheckEvent"/>.
    /// Cascade-deleted with its parent event (a finding has no meaning without the event).
    /// Inherits Id (PK), CreatedOn and CreatedBy audit columns from <see cref="BaseEntity"/>.
    /// Mapped to table "health_status" in the DbModel.
    /// </summary>
    public class HealthStatus : BaseEntity
    {
        [ForeignKey(nameof(HealthCheckEvent))]
        [Required]
        public Guid HealthCheckEventId { get; set; }

        [Required]
        public string ConditionName { get; set; }

        public string AffectedArea { get; set; }

        // Confidence in the range 0..1.
        [Column(TypeName = "decimal(5,4)")]
        public decimal Confidence { get; set; }

        // Severity: 1 (low) .. 3 (high).
        public int Severity { get; set; }

        // Navigation Property
        public virtual HealthCheckEvent HealthCheckEvent { get; set; }
    }
}
