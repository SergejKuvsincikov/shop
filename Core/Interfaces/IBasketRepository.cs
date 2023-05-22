using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IBasketRepository
    {
        Task<CustomerBasket?> GetBasketAcync(string id);
        Task<CustomerBasket?> UpdateBasketAcync(CustomerBasket basket);
        Task<bool> DeleteBasketAcync(string id);

    }
}