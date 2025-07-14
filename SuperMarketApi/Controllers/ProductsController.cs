using AutoMapper;
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
        public IActionResult GetAll([FromQuery] string? search, [FromQuery] string? category, [FromQuery] double? minPrice, [FromQuery] double? maxPrice)
        {
            try
            {
                var products = _productService.GetAllProducts(search, category, minPrice, maxPrice);
                return Ok(products);
            }
            catch(BadHttpRequestException ex)
            {
                return BadRequest(ex.ToString());
            }
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
            return Ok(product);


        }

        
    }
} 