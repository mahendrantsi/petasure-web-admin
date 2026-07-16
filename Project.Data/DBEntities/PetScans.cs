using Project.Core.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Data.DBEntities
{
    /// <summary>
    /// One row per recognition API call (register/similar/analyze/classify), across both
    /// dog and cat flows. PetId is nullable: a /similar or /analyze scan has no known pet
    /// until (and unless) the AI returns a matched ds_id — only /register carries a PetId
    /// up front. Columns mirror what the Python AI service's classify_and_route /
    /// similar / register responses actually return.
    /// Inherits Id (PK), CreatedOn and CreatedBy audit columns from <see cref="BaseEntity"/>.
    /// Mapped to table "pet_scans" in the DbModel.
    /// </summary>
    public class PetScans : BaseEntity
    {
        [ForeignKey(nameof(Pet))]
        public Guid? PetId { get; set; }

        public EnumPetScanType ScanType { get; set; }
        public EnumRecognitionSpecies Species { get; set; }

        // nose_image (register/similar/analyze) or the single `image` (classify)
        [ForeignKey(nameof(PrimaryImage))]
        public Guid? PrimaryImageId { get; set; }

        // dog_image/cat_image, when the call sends a second file
        [ForeignKey(nameof(SecondaryImage))]
        public Guid? SecondaryImageId { get; set; }

        // "dog" | "cat" | "reject", from classify_and_route
        public string RouteDecision { get; set; }

        // Raw classifier label, including "unknown"
        public string ClassifierLabel { get; set; }

        [Column(TypeName = "decimal(5,4)")]
        public decimal? ClassifierConfidence { get; set; }

        [Column(TypeName = "decimal(5,4)")]
        public decimal? ClassifierDogScore { get; set; }

        [Column(TypeName = "decimal(5,4)")]
        public decimal? ClassifierCatScore { get; set; }

        // "matched" | "possible_match" | "no_match" | null (n/a for register)
        public string MatchResult { get; set; }

        // similarity_value / distance returned by the AI service
        [Column(TypeName = "decimal(9,6)")]
        public decimal? MatchConfidence { get; set; }

        // Raw ds_id string the AI returned, kept even if Guid.TryParse fails
        public string MatchedDsId { get; set; }

        public bool IsBlurRejected { get; set; }
        public bool? IsNoseDetected { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string AiResponseRaw { get; set; }

        public int? AiStatusCode { get; set; }
        public int? AiRequestDurationMs { get; set; }

        public EnumPetScanStatus Status { get; set; }
        public string Notes { get; set; }

        // Navigation Properties
        public virtual PetInfo Pet { get; set; }
        public virtual PetImages PrimaryImage { get; set; }
        public virtual PetImages SecondaryImage { get; set; }
        public virtual ICollection<RecognitionErrors> RecognitionErrors { get; set; } = new List<RecognitionErrors>();
    }
}
