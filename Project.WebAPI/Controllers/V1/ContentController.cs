using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Project.Models.Content;
using Project.Services.IService;
using Project.Services.Service;

namespace Project.WebAPI.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : BaseController
    {
       
        private readonly IContentService _contentService;
        //Constructor for Pet Controller
        public ContentController(IContentService contentService)
        {

            _contentService = contentService;
        }


        [HttpPost("ContactUs")]
        public async Task<IActionResult> ContactUs(ContactUsRequestViewModel model)
        {
            try
            {
                var response = await _contentService.AddEnquiry(model, base.GetCurrentUserId());
                return response.IsSuccess ? this.Ok(response) : this.BadRequest(response);
            }
            catch (SecurityTokenException e)
            {
                return BadRequest(new { Message = e.Message + e.InnerException });
            }
        }

        [HttpGet("AppContent")]
        public async Task<IActionResult> AppContent()
        {
            try
            {
                var response = await _contentService.GetContentList();
                return response.IsSuccess ? this.Ok(response) : this.BadRequest(response);
            }
            catch (SecurityTokenException e)
            {
                return BadRequest(new { Message = e.Message + e.InnerException });
            }
        }


        [HttpGet("Faq")]
        public async Task<IActionResult> Faq()
        {
            try
            {
                var response = await _contentService.GetFaq();
                return response.IsSuccess ? this.Ok(response) : this.BadRequest(response);
            }
            catch (SecurityTokenException e)
            {
                return BadRequest(new { Message = e.Message + e.InnerException });
            }
        }
    }
}
