using Microsoft.AspNetCore.Mvc;
using StartApp.Repository.Dtos;
using StartApp.Repository.Infrastructure;
using StartApp.Service;

namespace StartApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public ListResult<ProductDto> Get(int offset, int limit)
        {
            var result = _productService.Get(offset, limit);
            return result;
        }

        [HttpGet]
        [Route("{id}")]
        public ProductDto Get(int id)
        {
            var result = _productService.Get(id);
            return result;
        }

        [HttpPost]
        public ProductDto Post([FromBody] ProductDto thingDto)
        {
           
            var result = _productService.Create(thingDto);
            return result;
        }
    }
}