using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PideYa.Server.Helpers;
using PideYa.Shared.Entities;

namespace PideYa.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DishCategoryController : ControllerBase
    {
        private readonly DataContext _context;

        public DishCategoryController(DataContext context)
        {
            _context = context;
        }

        // GET: api/DishCategory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DishCategory>>> GetDishesCategories()
        {
            if (_context.DishesCategories == null)
            {
                return NotFound();
            }
            return await _context.DishesCategories.ToListAsync();
        }

        // GET: api/DishCategory/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DishCategory>> GetDishCategory(int id)
        {
            if (_context.DishesCategories == null)
            {
                return NotFound();
            }
            var dishCategory = await _context.DishesCategories.FindAsync(id);

            if (dishCategory == null)
            {
                return NotFound();
            }

            return dishCategory;
        }

        // PUT: api/DishCategory/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDishCategory(int id, DishCategory dishCategory)
        {
            if (id != dishCategory.Id)
            {
                return BadRequest();
            }

            _context.Entry(dishCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DishCategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/DishCategory
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DishCategory>> PostDishCategory(DishCategory dishCategory)
        {
            if (_context.DishesCategories == null)
            {
                return Problem("Entity set 'DataContext.DishesCategories'  is null.");
            }
            _context.DishesCategories.Add(dishCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDishCategory", new { id = dishCategory.Id }, dishCategory);
        }

        // DELETE: api/DishCategory/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDishCategory(int id)
        {
            if (_context.DishesCategories == null)
            {
                return NotFound();
            }
            var dishCategory = await _context.DishesCategories.FindAsync(id);
            if (dishCategory == null)
            {
                return NotFound();
            }

            _context.DishesCategories.Remove(dishCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DishCategoryExists(int id)
        {
            return (_context.DishesCategories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
