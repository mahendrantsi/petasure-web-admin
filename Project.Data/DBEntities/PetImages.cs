using Project.Core.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Data.DBEntities
{
    /// <summary>
    /// One row per physical recognition image persisted to disk (nose crop, full-body/dog/cat
    /// image, face image). Separated from PetScans because a single scan (register/analyze)
    /// uploads two images, and both need independent storage metadata.
    /// Inherits Id (PK), CreatedOn and CreatedBy audit columns from <see cref="BaseEntity"/>.
    /// Mapped to table "pet_images" in the DbModel.
    /// </summary>
    public class PetImages : BaseEntity
    {
        // Nullable + SET NULL: unresolved until a match/registration completes (e.g. a
        // /similar check has no known pet until/unless the AI returns a matched ds_id).
        [ForeignKey(nameof(Pet))]
        public Guid? PetId { get; set; }

        public EnumImageKind ImageKind { get; set; }

        // Web-relative path, e.g. "/uploads/recognition/{guid}.jpg" — servable via
        // Project.WebAPI's UseStaticFiles mapping.
        [Required]
        public string StoragePath { get; set; }

        public string OriginalFileName { get; set; }
        public string ContentType { get; set; }
        public long? FileSizeBytes { get; set; }

        // Navigation Property
        public virtual PetInfo Pet { get; set; }
    }
}
