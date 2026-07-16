using Project.Core.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Data.DBEntities
{
    /// <summary>
    /// A single failure recorded against a <see cref="PetScans"/> attempt (image save failed,
    /// AI request failed/timed out, AI response could not be parsed, or the DB save itself
    /// failed). Distinct from the generic ExceptionLogger table — this one is scoped to the
    /// recognition domain so the Admin Dashboard's error-breakdown metrics can query it directly.
    /// Cascade-deleted with its parent scan (an error has no meaning without the scan it belongs to).
    /// Inherits Id (PK), CreatedOn and CreatedBy audit columns from <see cref="BaseEntity"/>.
    /// Mapped to table "recognition_errors" in the DbModel.
    /// </summary>
    public class RecognitionErrors : BaseEntity
    {
        [ForeignKey(nameof(PetScan))]
        [Required]
        public Guid PetScanId { get; set; }

        public EnumRecognitionErrorStage ErrorStage { get; set; }

        [Required]
        public string ErrorMessage { get; set; }

        public int? StatusCodeReturned { get; set; }

        // Navigation Property
        public virtual PetScans PetScan { get; set; }
    }
}
