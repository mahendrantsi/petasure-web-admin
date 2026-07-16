using Project.Models.GeneralModel;
using System.Linq;

namespace Project.Services.Service
{
    /// <summary>
    /// Maps raw Python AI output to mobile-facing guidance (T29). The disclaimer on
    /// <see cref="IllHealthResponse"/> is set by that class's own default, so every path
    /// through this mapper carries it without needing to be repeated here.
    /// </summary>
    public static class IllHealthGuidanceMapper
    {
        public static IllHealthResponse Map(IllHealthAiResult aiResult)
        {
            var response = new IllHealthResponse();

            if (aiResult == null)
            {
                response.GuidanceText = "We could not process this image right now. Please try again.";
                response.SeverityLevel = "Unknown";
                response.RecommendedAction = "Retake the photo in good lighting and try again shortly.";
                return response;
            }

            // Pass through the AI's own flags on every non-null path.
            response.IsStub = aiResult.IsStub;
            response.ImageUnclear = aiResult.ImageUnclear;
            response.SpeciesMismatch = aiResult.SpeciesMismatch;
            response.DetectedSpecies = aiResult.DetectedSpecies;

            response.Conditions = aiResult.Conditions.Select(c => new IllHealthConditionDto
            {
                ConditionName = c.ConditionName,
                AffectedArea = c.AffectedArea,
                Confidence = c.Confidence,
                Severity = c.Severity,
            }).ToList();

            if (aiResult.ImageUnclear)
            {
                response.GuidanceText = "The image was not clear enough to analyze.";
                // "Low confidence" is the contract's preferred retake signal (alongside image_unclear=true).
                response.SeverityLevel = "Low confidence";
                response.RecommendedAction = "Retake the photo in good lighting, holding the camera steady.";
                return response;
            }

            if (!aiResult.Conditions.Any())
            {
                response.GuidanceText = aiResult.Summary ?? "No signs of concern were detected in this image.";
                response.SeverityLevel = "None";
                response.RecommendedAction = "Continue routine monitoring. Consult a vet if you notice any changes.";
                return response;
            }

            var maxConfidence = aiResult.Conditions.Max(c => c.Confidence);
            var maxSeverity = aiResult.Conditions.Max(c => c.Severity);

            if (maxConfidence < IllHealthConstants.LowConfidenceThreshold)
            {
                response.GuidanceText = "Possible signs were detected, but confidence is too low for a reliable result.";
                response.SeverityLevel = "Low confidence";
                response.RecommendedAction = "Monitor your pet and consult a vet if symptoms persist or worsen.";
                return response;
            }

            response.SeverityLevel = maxSeverity switch
            {
                3 => "High",
                2 => "Medium",
                _ => "Low",
            };

            response.GuidanceText = aiResult.Summary ?? "Possible signs of a health issue were detected.";

            response.RecommendedAction = maxSeverity switch
            {
                3 => "Contact a veterinarian as soon as possible.",
                2 => "Schedule a vet visit in the next few days.",
                _ => "Keep an eye on this area and consult a vet if it does not improve.",
            };

            return response;
        }
    }
}
