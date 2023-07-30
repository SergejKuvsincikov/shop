using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAgregate;
using Core.Interfaces;

namespace Infrastructure.Services
{

    public class OrderService : IOrderService
    {
        public IGenericRepository<Order> _orderRepo { get; }
        public IGenericRepository<DeliveryMethod> _deliveryMethodRepo { get; }
        public IGenericRepository<Product> _productRepo { get; }
        public IBasketRepository _basketRepo { get; set; }
        
        public OrderService(IGenericRepository<Order> orderRepo, IGenericRepository<DeliveryMethod> deliveryMethodRepo,
            IGenericRepository<Product> productRepo, IBasketRepository basketRepo) 
        {
            _basketRepo = basketRepo;
            _productRepo = productRepo;
            _deliveryMethodRepo = deliveryMethodRepo;
            _orderRepo = orderRepo;
        }
            
        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address ahippingAddress)
        {
            var basket = await _basketRepo.GetBasketAcync(basketId);
            var items = new List<OrderItem>();
            foreach(var item in basket.Items)
            {
                var productIem = await _productRepo.GetByIdAsync(int.Parse(item.Id));
                var itemOrdered = new ProductItemOrdered(productIem.Id,productIem.Name,productIem.PictureUrl);
                var orderItem = new OrderItem(itemOrdered, productIem.Price, item.Quantity);
                items.Add(orderItem);


            }
            
            var deliveryMethod =  await _deliveryMethodRepo.GetByIdAsync(deliveryMethodId);
            var subTotal = items.Sum(x => x.Price * x.Quantity);

            var order = new Order(items,buyerEmail,ahippingAddress,deliveryMethod,subTotal);

            // TODO : save to DB

            return order;
        }

        public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            throw new NotImplementedException();
        }
    }
}