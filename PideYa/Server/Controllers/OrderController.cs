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
            return await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Dish)
                .OrderByDescending(o => o.Id)
                .FirstOrDefaultAsync(
                o => o.User.Id == id &&
                (o.Status == OrderStatus.Pending || o.Status == OrderStatus.Processing));
        }

        [HttpGet("/api/OrderAddDetail")]
        public async Task<ActionResult<Order?>> AddDetailOrder(int orderId, int dishId, int quantity = 1)
        {
            if (!await _context.Dishes.AnyAsync(d => d.id == dishId))
            {
                return NotFound();
            }

            var dish = await _context.Dishes.FirstOrDefaultAsync(d => d.id == dishId);
            Order? order;
            if (!await _context.Orders.AnyAsync(o => o.Id == orderId && o.Status == OrderStatus.Pending))
            {
                order = new Order
                {
                    CreatedAt = DateTime.Now,
                    Status = OrderStatus.Pending,
                    Total = 0,
                    UserId = 1,
                    OrderDetails = new List<OrderDetail>
                    {
                        new()
                        {
                            Dish = dish,
                            Quantity = quantity,
                            SubTotal = dish.Price * quantity
                        }
                    },
                };
                _context.Orders.Add(order);
            }
            else
            {
                order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == orderId);
                order.OrderDetails.Add(new()
                {
                    Dish = dish,
                    Quantity = quantity,
                    SubTotal = dish.Price * quantity,
                });
                _context.Entry(order).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
            return Ok(order);
        }

        [HttpGet("/api/OrderDeleteDetail")]
        public async Task<ActionResult<Order?>> DeleteDetailOrder(int orderId, int detailId)
        {
            if (!await _context.Orders.AnyAsync(o => o.Id == orderId && o.Status == OrderStatus.Pending))
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Dish)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order is not null && order.OrderDetails.Any(d => d.Id == detailId))
            {
                order.OrderDetails.Remove(order.OrderDetails.Single(d => d.Id == detailId));
                await _context.SaveChangesAsync();
            }

            return Ok(order);
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
