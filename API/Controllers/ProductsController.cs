using API.DTOs;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specificotions;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
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
        public async Task<ActionResult<Pagination<ProductToReturnDto1>>> GetProducts([FromQuery]ProductSpecParams param)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(param);
            var countSpec = new ProductWithFiltersAndCountSpecification(param);

            var totalItems = await _productRepository.CountAsync(countSpec); 
            var products = await _productRepository.ListAsync(spec);


            if(products.Count < 1) 
            {
                return NotFound(new ApiResponse(404));
            }

            var data = _mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductToReturnDto1>>(products);
            var dataPaged = new Pagination<ProductToReturnDto1>(param.PageIndex, param.PageSize, totalItems,data);
            return Ok(dataPaged) ;
        } 

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto1>> GetProduct(int id)
        { 
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await _productRepository.GetEntityWithSpec(spec);
            if(product == null) 
            {
                return NotFound(new ApiResponse(404));
            }
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