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
    public class OrderController : ControllerBase
    {
        private readonly DataContext _context;

        public OrderController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("/api/OrderByEmployee/{id}")]
        public async Task<ActionResult<Order?>> GetActiveOrderByEmployee(int id)
        {
            if (!await _context.Orders.AnyAsync(o => o.User.Id == id))
            {
                return NotFound();
            }
            return await _context.Orders.FirstOrDefaultAsync(
                o => o.User.Id == id &&
                (o.Status == OrderStatus.Pending || o.Status == OrderStatus.Processing));
        }

        [HttpGet("/api/OrderAddDetail")]
        public async Task<ActionResult<Order?>> AddDetailOrder(int orderId, int dishId, int quantity = 1)
        {
            var existingOrder = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(
                o => o.Id == orderId &&
                (o.Status == OrderStatus.Pending || o.Status == OrderStatus.Processing)
            );
            if (existingOrder is null)
            {
                return NotFound("Order not found");
            }

            var existingDish = await _context.Dishes.FirstOrDefaultAsync(d => d.id == dishId);
            if (existingDish is null)
            {
                return NotFound("Dish not found");
            }

            if (quantity <= 0) quantity = 1;
            existingOrder.OrderDetails.Add(new()
            {
                Dish = existingDish,
                Quantity = quantity,
                SubTotal = existingDish.Price * quantity
            });

            await _context.SaveChangesAsync();
            return Ok(existingOrder);
        }

        // GET: api/Order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            return await _context.Orders.ToListAsync();
        }

        // GET: api/Order/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Order/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST: api/Order
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'DataContext.Orders'  is null.");
            }
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        // DELETE: api/Order/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
