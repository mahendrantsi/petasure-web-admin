using Microsoft.AspNetCore.Http;

namespace Project.Models.GeneralModel
{
    public class IndexModel
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Search { get; set; }
        public string OrderBy { get; set; }
        public bool OrderByAsc { get; set; }
        public bool IsPostBack { get; set; }
        public string DynamicSearch { get; set; }
        public IndexModel()
        {
            PageSize = 10;
            IsPostBack = false;
            OrderByAsc = true;
            Page = 1;
        }
    }

    public class SimilarDogRequestViewModel
    {
        public IFormFile Image { get; set; }
        // The species the user selected in the app ("dog"/"cat"). Forwarded to the Python
        // AI service so it can hard-reject a photo whose detected species doesn't match
        // (or isn't a pet at all). Optional for backward compatibility with older clients.
        public string Species { get; set; }
    }


    public class RegisterDogRequestViewModel
    {
        public IFormFile NoseImage { get; set; }
        public IFormFile DogImage { get; set; }
        // Optional: 3-view images added in new mobile app version
        public IFormFile LeftViewImage { get; set; }
        public IFormFile RightViewImage { get; set; }
        public IFormFile TopViewImage { get; set; }
        public string PetId { get; set; }
        public string Species { get; set; }
    }

    public class AnalyzeDogRequestViewModel
    {
        public IFormFile NoseImage { get; set; }
        public IFormFile DogImage { get; set; }
        public string Species { get; set; }
    }

    public class SimilarCatRequestViewModel
    {
        public IFormFile Image { get; set; }
        public string Species { get; set; }
    }

    public class RegisterCatRequestViewModel
    {
        // Cats are identified by nose biometrics (same pipeline as dogs); the AI service
        // auto-classifies the species. NoseImage = close-up nose crop, CatImage = full-body photo.
        public IFormFile NoseImage { get; set; }
        public IFormFile CatImage { get; set; }
        // Optional: 3-view images added in new mobile app version
        public IFormFile LeftViewImage { get; set; }
        public IFormFile RightViewImage { get; set; }
        public IFormFile TopViewImage { get; set; }
        public string PetId { get; set; }
        public string Species { get; set; }
    }

    public class AnalyzeCatRequestViewModel
    {
        public IFormFile NoseImage { get; set; }
        public IFormFile CatImage { get; set; }
        public string Species { get; set; }
    }
}
