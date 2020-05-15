using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngularApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Razor.Language;
using SQLitePCL;

namespace AngularApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly ShoppingDbContext _context;

        public OrdersController(ShoppingDbContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminOrdersViewModel>>> GetOrders()
        {

            var result =
                          (from u in _context.Users
                           join
 a in _context.Orders on u.Id equals a.UserId
                           join h in _context.OrderDetails on a.Id equals h.OrderId
                           join c in _context.Products on h.ProductId equals c.Id

                           select new AdminOrdersViewModel
                           {
                               Id = a.Id,
                               OrderDate = a.OrderDate,
                               TotalPrice = a.TotalPrice,
                               Status = a.Status,
                               ProductName = c.ProductName,
                               UserName = u.UserName
                           }).ToListAsync();
            return await result;
        }

        //public List<AdminOrdersViewModel> GetOrders()
        //{

        //    var result =
        //                  (from u in _context.Users join
        //                  a in _context.Orders on u.Id equals a.UserId
        //                   join h in _context.OrderDetails on a.Id equals h.OrderId
        //                   join c in _context.Products on h.ProductId equals c.Id

        //                   select new AdminOrdersViewModel
        //                   {
        //                       Id = a.Id,
        //                       OrderDate = a.OrderDate,
        //                       TotalPrice = a.TotalPrice,
        //                       Status = a.Status,
        //                       ProductName = c.ProductName,
        //                       UserName = u.UserName
        //                   }).OrderByDescending(i => i.Id).ToList<AdminOrdersViewModel>();
        //    return result;
        //}

        [HttpGet("{id}")]
        public async Task<ActionResult<Orders>> GetOrders(int id)
        {
            var orders = await _context.Orders.FindAsync(id);

            if (orders == null)
            {
                return NotFound();
            }

            return orders;
        }
        [HttpGet]
       [Route("userorders")]
        public IEnumerable<Orders> GetUserOrder([FromQuery]string uid)
        {
            var orders = _context.Orders.Where(i => i.UserId == uid && i.Isdeleted == false).ToList();

            if (orders == null)
            {
                return null;
            }

            return orders;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<ActionResult<IEnumerable<AdminOrdersViewModel>>> PutOrders(int id, Orders orders)
        {
            if (id != orders.Id)
            {
                // return BadRequest();
            }

            _context.Entry(orders).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
                if (orders.Status == 3)
                {
                    //products id in order

                    var list = _context.OrderDetails.Where(o => o.OrderId == orders.Id).ToList();
                    //products
                    var products = _context.Products.ToList();
                    foreach (var item in list)
                    {
                        var obj = products.Find(i => i.Id == item.ProductId);
                        obj.Quantity = obj.Quantity + item.Quantity;


                    }
                }
              await  _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrdersExists(id))
                {
                    //  return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return await GetOrders();
           
        }

        // POST: api/Orders
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
       
        [HttpPost]
        [Route("submitorder")]
        public int PostOrders(Orders orders)
        {
            _context.Orders.Add(orders);
             _context.SaveChanges();
            return orders.Id;
            // return CreatedAtAction("GetOrders", new { id = orders.Id }, orders);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Orders>> DeleteOrders(int id)
        {
            var orders = await _context.Orders.FindAsync(id);
            if (orders == null)
            {
                return NotFound();
            }

            else
            {
                orders.Isdeleted = true;
                await  _context.SaveChangesAsync();
            }

            return Ok();
        }

        private bool OrdersExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
