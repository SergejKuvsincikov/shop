using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specificotions;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        public IGenericRepository<Product> _productRepository { get; }
        public IGenericRepository<ProductBrand> _productBrandRepository { get; }
        public IGenericRepository<ProductType> _productTypeRepository { get; }
                public IMapper _mapper { get; }
        public ProductsController(IGenericRepository<Product> productRepository, 
            IGenericRepository<ProductBrand> productBrandRepository, IGenericRepository<ProductType> productTypeRepository,

            IMapper mapper)
        {
            _mapper = mapper;
            _productTypeRepository = productTypeRepository;
            _productBrandRepository = productBrandRepository;
            _productRepository = productRepository;

        }

         [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto1>>> GetProducts()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification();
            var products = await _productRepository.ListAsync(spec);
            return Ok(_mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductToReturnDto1>>(products)) ;
        } 

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto1>> GetProduct(int id)
        { 
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await _productRepository.GetEntityWithSpec(spec);
            return Ok(_mapper.Map<Product,ProductToReturnDto1>(product)
            );
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