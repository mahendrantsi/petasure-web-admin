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
    }


    public class RegisterDogRequestViewModel
    {
        public IFormFile NoseImage { get; set; }
        public IFormFile DogImage { get; set; }
        public string PetId { get; set; }
    }

    public class AnalyzeDogRequestViewModel
    {
        public IFormFile NoseImage { get; set; }
        public IFormFile DogImage { get; set; }
    }

    public class SimilarCatRequestViewModel
    {
        public IFormFile Image { get; set; }
    }

    public class RegisterCatRequestViewModel
    {
        // Cats are identified by nose biometrics (same pipeline as dogs); the AI service
        // auto-classifies the species. NoseImage = close-up nose crop, CatImage = full-body photo.
        public IFormFile NoseImage { get; set; }
        public IFormFile CatImage { get; set; }
        public string PetId { get; set; }
    }

    public class AnalyzeCatRequestViewModel
    {
        public IFormFile NoseImage { get; set; }
        public IFormFile CatImage { get; set; }
    }
}
