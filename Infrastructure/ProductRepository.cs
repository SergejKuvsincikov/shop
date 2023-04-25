using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ProductRepository : IProductRepository
    {
        public StoreContext _storeContext { get; }
        public ProductRepository(StoreContext storeContext)
        {
            _storeContext = storeContext;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _storeContext.Products.Include(p=>p.ProductType).Include(p=>p.ProductBrand).FirstOrDefaultAsync(p=> p.Id == id);
        }

        public async Task<IReadOnlyCollection<Product>> GetProductsAsync()
        {
            return await _storeContext.Products.Include(p=>p.ProductType).Include(p=>p.ProductBrand).ToListAsync(); ;
        }

        public async Task<IReadOnlyCollection<ProductBrand>> GetProductBrandsAsync()
        {
            return await _storeContext.ProductBrands.ToListAsync(); ;
        }

        public async Task<IReadOnlyCollection<ProductType>> GetProductTypesAsync()
        {
            return await _storeContext.ProductTypes.ToListAsync(); ;
        }
    }
}