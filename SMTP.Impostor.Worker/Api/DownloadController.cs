using Microsoft.AspNetCore.Mvc;

namespace SMTP.Impostor.Worker.Api
{
    public class DownloadController : Controller
    {
        [HttpGet("/download")]
        //[Produces("application/json")]
        public IActionResult Index()
        {
            return Ok("Hiya");
        }
    }
}