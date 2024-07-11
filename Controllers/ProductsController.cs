using MarketApi.Models;
using MarketApi.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace MarketApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("_myAllowSpecificOrigins")]
    public class ProductsController : ControllerBase
    {
        private readonly IMarketService _marketService;

        public ProductsController(IMarketService marketService)
        {
            _marketService = marketService;
        }

        [HttpPost("search")]
        public async Task<ActionResult<List<Product>>> GetProducts([FromBody] ProductFilter productFilter)
        {
            var products = await _marketService.GetProductsAsync(productFilter.KeyWord);
            return Ok(products);
        }
    }
}
