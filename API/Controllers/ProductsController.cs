using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        public IGenericRepository<Product> _productRepository { get; }
        public IGenericRepository<ProductBrand> _productBrandRepository { get; }
        public IGenericRepository<ProductType> _productTypeRepository { get; }
        public ProductsController(IGenericRepository<Product> productRepository, 
            IGenericRepository<ProductBrand> productBrandRepository, IGenericRepository<ProductType> productTypeRepository)
        {
            _productTypeRepository = productTypeRepository;
            _productBrandRepository = productBrandRepository;
            _productRepository = productRepository;

        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _productRepository.GetAllAsync();
            return Ok(products) ;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        { 
            var product = await _productRepository.GetByIdAsync(id);
            return Ok(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<List<ProductBrand>>> GetBrands()
        {
            var brands = await _productBrandRepository.GetAllAsync();
            return Ok(brands) ;
        }
        
        [HttpGet("types")]
        public async Task<ActionResult<List<ProductType>>> GetTypes()
        {
            var types = await _productTypeRepository.GetAllAsync();
            return Ok(types) ;
        }
    }
}