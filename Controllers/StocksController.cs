using CrudWithMongoDB.Model;
using CrudWithMongoDB.Service;
using Microsoft.AspNetCore.Mvc;

namespace CrudWithMongoDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly StocksService _stockService;
        public StocksController(StocksService stocksService)
        {
            _stockService = stocksService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStocks()
        {
            List<Stocks> allStocks = await _stockService.GetAsync();
            return Ok(allStocks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStockById(string id)
        {
            try
            {
                var data = await _stockService.GetAsync(id);
                if (data != null)
                {
                    return NotFound();
                }
                return Ok(data);
            } catch(Exception ex)
            {
                return StatusCode(500, new { ErrorMsg = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddStocks([FromBody] Stocks stocks)
        {
            try
            {
                var data = await _stockService.CreateAsync(stocks);
                return StatusCode(StatusCodes.Status201Created, data);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMsg = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStock([FromBody] Stocks newStock)
        {
            try
            {
                var data = await _stockService.UpdateAsync(newStock.Id, newStock);
                return Ok(data);
            } catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMsg = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStock([FromRoute] string id)
        {
            try
            {
                await _stockService.RemoveAsync(id);
                return StatusCode(StatusCodes.Status200OK);
            } catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMsg = ex.Message });
            }
        }
    }
}
