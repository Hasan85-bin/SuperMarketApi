using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SuperMarketApi.DTOs.Product;
using SuperMarketApi.Services;
using System.Reflection;


namespace SuperMarketApi.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] string? category, [FromQuery] double? minPrice, [FromQuery] double? maxPrice)
        {
            try
            {
                var products = await _productService.GetAllProductsAsync(search, category, minPrice, maxPrice);
                return Ok(products);
            }
            catch(BadHttpRequestException ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "Staff,Admin")]
        public async Task<IActionResult> Create([FromBody] CreateProductDto newProduct)
        {
            var product = await _productService.AddProductAsync(newProduct);
            return CreatedAtAction(nameof(GetById), new { id = product.ID }, product);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Staff,Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] JsonPatchDocument<UpdateProductDto> patchDoc)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if(product == null)
            {
                return NotFound();
            }
            var updateDto = _mapper.Map<UpdateProductDto>(product);
            patchDoc.ApplyTo(updateDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!TryValidateModel(updateDto))
            {
                return BadRequest(ModelState);
            }
            _mapper.Map(updateDto, product);
            await _productService.UpdateProductAsync(id, updateDto);
            return Ok(product);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _productService.DeleteProductAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
        
    }
} 