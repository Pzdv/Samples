using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindDAL.Enum;
using NorthwindDAL.Model;

namespace OrdersService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly NorthwindContext _context;

        public OrdersController(NorthwindContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Getting a list of all orders (without details).
        /// </summary>
        [HttpGet]
        public IActionResult GetOrders()
        {
            var orders = _context.Orders;
            return Ok(orders.ToList());
        }

        /// <summary>
        /// Getting detailed information about a specific order by id.
        /// </summary>
        /// <param name="id"></param>
        /// <response code="404">Order with this id was not found.</response>
        [HttpGet("{id:int}")]
        public IActionResult GetOrder(int id)
        {
            var order = _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefault(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        /// <summary>
        /// Adding a new order, including Order Details.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///      {
        ///         "customerId": "VINET",
        ///         "employeeId": 5,
        ///         "requiredDate": "1996-08-01T00:00:00",
        ///         "shipVia": 3,
        ///         "freight": 32.3800,
        ///         "shipName": "Vins et alcools Chevalier",
        ///         "shipAddress": "59 rue de l'Abbaye",
        ///         "shipCity": "Reims",
        ///         "shipPostalCode": "51100",
        ///         "shipCountry": "France",
        ///         "state": 2,
        ///         "orderDetails": [
        ///             {
        ///                 "productId": 11,
        ///                 "unitPrice": 14.0000,
        ///                 "quantity": 12,
        ///                 "discount": 0
        ///             }
        ///         ]
        ///     }
        ///
        /// </remarks>
        /// <param name="order"></param>
        /// <response code="201">Returns the newly created item</response>
        [HttpPost]
        public IActionResult Create([FromBody] Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
            return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
        }

        /// <summary>
        /// Deletes a specific Order by id.
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">The deletion was successful</response>
        /// <response code="404">The order with this id does not exist.</response> 
        /// <response code="400">The order state is not 'New'</response>
        /// <returns>Deleted order</returns>
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var order = _context.Orders.Include(o => o.OrderDetails).FirstOrDefault(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            if (order.State != OrderState.New)
            {
                return BadRequest();
            }

            _context.Orders.Remove(order);

            _context.SaveChanges();

            return Ok(order);
        }

        /// <summary>
        /// Update the order in the "New" status.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="order"></param>
        /// <response code="404">The order with this id does not exist.</response>
        /// <response code="400">The order state is not 'New' or parameter id does not match OrderId in request body</response>
        /// <response code="200">Order has been successfully updated</response>
        /// <returns>Updated order.</returns>
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] Order order)
        {
            var existOrder = _context.Orders.Include(o => o.OrderDetails).FirstOrDefault(o => o.OrderId == id);

            if (existOrder == null)
            {
                return NotFound();
            }

            if (existOrder.State != OrderState.New || id != order.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(existOrder).State = EntityState.Detached;

            order.OrderDate = existOrder.OrderDate;
            order.ShippedDate = existOrder.ShippedDate;

            _context.Entry(order).State = EntityState.Modified;

            _context.SaveChanges();

            return Ok(order);
        }
    }
}
