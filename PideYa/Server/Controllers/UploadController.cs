using Microsoft.AspNetCore.Mvc;
using PideYa.Server.Helpers;
using System.Net.Http.Headers;

namespace PideYa.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : Controller
    {
        private readonly DataContext context;

        public UploadController(DataContext context)
        {
            this.context = context;
        }

        [HttpPost("{id}"), DisableRequestSizeLimit]
        public IActionResult Upload(int id)
        {
            try
            {
                var dish = context.Dishes.FirstOrDefault(o => o.id == id);

                if (dish is null) return NotFound();

                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    //var fullPath = Path.Combine(pathToSave, fileName);
                    var fullPath = Path.Combine(pathToSave, string.Concat(id.ToString(), Path.GetExtension(Path.Combine(pathToSave, fileName))));
                    var dbPath = Path.Combine(folderName, string.Concat(id.ToString(), Path.GetExtension(Path.Combine(pathToSave, fileName))));
                    //var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    context.DishImages.Add(new Shared.Entities.DishImage()
                    {
                        id = id,
                        Dish = dish,
                        url = dbPath
                    });

                    context.SaveChanges();

                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
