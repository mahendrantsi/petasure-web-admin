using System.ComponentModel;

namespace Project.Core.Enum
{
    /// <summary>
    /// Which recognition API call produced a <see cref="Project.Data.DBEntities.PetScans"/> row.
    /// </summary>
    public enum EnumPetScanType
    {
        [Description("Register")]
        Register = 1,
        [Description("Similar")]
        Similar = 2,
        [Description("Analyze")]
        Analyze = 3,
        [Description("Classify")]
        Classify = 4,
    }

    /// <summary>
    /// Species detected/claimed for a scan. Distinct from EnumHealthCheckSpecies because
    /// recognition can produce "Unknown" (classifier rejected the image / low confidence),
    /// which never happens in the illness flow.
    /// </summary>
    public enum EnumRecognitionSpecies
    {
        [Description("Dog")]
        Dog = 1,
        [Description("Cat")]
        Cat = 2,
        [Description("Unknown")]
        Unknown = 3,
    }

    /// <summary>
    /// Which physical image slot a stored <see cref="Project.Data.DBEntities.PetImages"/> row represents.
    /// </summary>
    public enum EnumImageKind
    {
        [Description("Nose Image")]
        NoseImage = 1,
        [Description("Full Body Image")]
        FullBodyImage = 2,
        [Description("Face Image")]
        FaceImage = 3,
        [Description("Left View Image")]
        LeftViewImage = 4,
        [Description("Right View Image")]
        RightViewImage = 5,
        [Description("Top View Image")]
        TopViewImage = 6,
    }

    /// <summary>
    /// Outcome of a recognition scan attempt.
    /// </summary>
    public enum EnumPetScanStatus
    {
        [Description("Success")]
        Success = 1,
        [Description("Rejected")]
        Rejected = 2,
        [Description("Failed")]
        Failed = 3,
        [Description("Pending Review")]
        PendingReview = 4,
    }

    /// <summary>
    /// Stage at which a recognition scan failed, for RecognitionErrors.ErrorStage.
    /// </summary>
    public enum EnumRecognitionErrorStage
    {
        [Description("Image Save")]
        ImageSave = 1,
        [Description("AI Request")]
        AiRequest = 2,
        [Description("AI Response Parse")]
        AiResponseParse = 3,
        [Description("Database Save")]
        DbSave = 4,
        [Description("Recognition Gate")]
        RecognitionGate = 5,
    }
}
