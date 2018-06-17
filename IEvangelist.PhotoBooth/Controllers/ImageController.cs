using System.Threading.Tasks;
using IEvangelist.PhotoBooth.Models;
using IEvangelist.PhotoBooth.Services;
using Microsoft.AspNetCore.Mvc;

namespace IEvangelist.PhotoBooth.Controllers
{
    [Route("api/image")]
    public class ImageController : Controller
    {
        [HttpPost, Route("generate")]
        public async Task<IActionResult> Generate(
            [FromBody] ImagesPostRequest imagesPostRequest,
            [FromServices] IImageProcessorService imageProcessor)
        {
            var response = await imageProcessor.ProcessImagesAsync(imagesPostRequest);
            return Json(response);
        }

        [HttpGet, Route("options")]
        public IActionResult GetOptions(
            [FromServices] IImageProcessorService imageProcessor)
            => Json(imageProcessor.GetImageOptions());
    }
}
