using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Specificotions
{
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        public ProductsWithTypesAndBrandsSpecification(ProductSpecParams param ) : base(

            x => 
            (string.IsNullOrEmpty(param.Search) || x.Name.ToLower().Contains(param.Search)) &&
            (!param.BrandId.HasValue || x.ProductBrandId == param.BrandId) && 
            (!param.TypeId.HasValue || x.ProductTypeId == param.TypeId)
        )
        {
            AddInclude(x=>x.ProductBrand);
            AddInclude(x=>x.ProductType);
            
             
            if(!string.IsNullOrEmpty(param.Sort))
            {
                switch (param.Sort)
                {
                    case "PriceAsc":
                        AddOrderBy(x=>x.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDesc(x=>x.Price);
                        break;
                    default: 
                        AddOrderBy(x => x.Name);
                        break;                        
                }
            }

            ApplyPaging(param.PageSize * (param.PageIndex -1), param.PageSize);
        }

        public ProductsWithTypesAndBrandsSpecification(int id) : base (x => x.Id==id)
        {
            AddInclude(x=>x.ProductBrand);
            AddInclude(x=>x.ProductType);
        }
    }
}