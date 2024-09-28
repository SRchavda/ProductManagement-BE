using CrudWithMongoDB.Model;
using CrudWithMongoDB.Service;
using Microsoft.AspNetCore.Mvc;

namespace CrudWithMongoDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;
        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var a = await _orderService.GetAsync();
            return Ok(a);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(string id)
        {
            try
            {
                var data = await _orderService.GetAsync(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMsg = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] Orders orders)
        {
            try
            {
                var data = await _orderService.CreateAsync(orders);
                return StatusCode(StatusCodes.Status201Created, data);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMsg = ex.Message });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder([FromRoute] string id)
        {
            try
            {
                await _orderService.RemoveAsync(id);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMsg = ex.Message });
            }
        }
    }
}
