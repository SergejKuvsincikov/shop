using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entities.OrderAgregate;

namespace Core.Specificotions
{
    public class OrdersWithItemsAndOrderingSpecifications : BaseSpecification<Order>
    {
        public OrdersWithItemsAndOrderingSpecifications(string email) : base(o => o.BuyerEmail == email)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
            AddOrderByDesc(o => o.OrderDate);
        }

        public OrdersWithItemsAndOrderingSpecifications(int id,string email) : 
            base(o => o.BuyerEmail == email && o.Id == id)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
        }
    }
}