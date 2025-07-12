using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SuperMarketApi.DTOs;
using SuperMarketApi.Services;

namespace SuperMarketApi.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] string? search, [FromQuery] string? category, [FromQuery] double? minPrice, [FromQuery] double? maxPrice)
        {
            var products = _productService.GetAllProducts(search, category, minPrice, maxPrice);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateProductDto newProduct)
        {
            var product = _productService.AddProduct(newProduct);
            return CreatedAtAction(nameof(GetById), new { id = product.ID }, product);
        }

        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] JsonPatchDocument<UpdateProductDto> patchDoc)
        {
            var product = _productService.GetProductById(id);
            if(product == null)
            {
                return NotFound();
            }


        }
    }
} 