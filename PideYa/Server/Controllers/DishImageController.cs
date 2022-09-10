using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PideYa.Server.Helpers;
using PideYa.Shared.Entities;

namespace PideYa.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishImageController : Controller
    {
        private readonly DataContext _context;

        public DishImageController(DataContext context)
        {
            _context = context;
        }

        // GET: api/DishImage/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DishImage>> GetDishImage(int id)
        {
            if (_context.DishImages == null)
            {
                return NotFound();
            }
            var dishImage = await _context.DishImages
                .Include(d => d.Dish)
                .FirstOrDefaultAsync(d => d.id == id);

            if (dishImage == null)
            {
                return NotFound();
            }

            return dishImage;
        }
    }
}
