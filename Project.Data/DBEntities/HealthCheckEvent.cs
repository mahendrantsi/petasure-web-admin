using Project.Core.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Data.DBEntities
{
    /// <summary>
    /// Stores every uploaded ill-health / early-issue-detection image and its AI result
    /// so a dataset accrues over time (Doc 2). Inherits Id (PK), CreatedOn and CreatedBy
    /// audit columns from <see cref="BaseEntity"/> — the same audit convention every other
    /// table uses. Mapped to table "health_check_events" in the DbModel.
    /// </summary>
    public class HealthCheckEvent : BaseEntity
    {
        // FK to the existing pet profile (PetInfo). Nullable + SET NULL on delete, matching
        // the repo's PetInfo FK convention so the image/result is retained for the dataset
        // even if the pet profile is later removed.
        [ForeignKey(nameof(Pet))]
        public Guid? PetId { get; set; }

        public EnumHealthCheckSpecies Species { get; set; }

        [Required]
        public string ImageRef { get; set; }

        public string PreviousImageRef { get; set; }

        public DateTime SubmittedAt { get; set; }

        public EnumHealthCheckStatus Status { get; set; } = EnumHealthCheckStatus.Pending;

        [Column(TypeName = "nvarchar(max)")]
        public string AiSummary { get; set; }

        public bool DisclaimerShown { get; set; }

        public string ModelVersion { get; set; }

        // Navigation Properties
        public virtual PetInfo Pet { get; set; }
        public virtual ICollection<HealthStatus> HealthStatuses { get; set; } = new List<HealthStatus>();
    }
}
