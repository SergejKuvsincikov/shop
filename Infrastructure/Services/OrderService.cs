using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAgregate;
using Core.Interfaces;
using Core.Specificotions;

namespace Infrastructure.Services
{

    public class OrderService : IOrderService
    {
        public IUnitOfWork _unitOfWork { get; }
        public IBasketRepository _basketRepo { get; set; }
        
        public OrderService(IUnitOfWork unitOfWork, IBasketRepository basketRepo) 
        {
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
        }
            
        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address ahippingAddress)
        {
            var basket = await _basketRepo.GetBasketAcync(basketId);
            var items = new List<OrderItem>();
            foreach(var item in basket.Items)
            {
                var productIem = await _unitOfWork.Repository<Product>().GetByIdAsync(int.Parse(item.Id));
                var itemOrdered = new ProductItemOrdered(productIem.Id,productIem.Name,productIem.PictureUrl);
                var orderItem = new OrderItem(itemOrdered, productIem.Price, item.Quantity);
                items.Add(orderItem);


            }
            
            var deliveryMethod =  await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
            var subTotal = items.Sum(x => x.Price * x.Quantity);

            var order = new Order(items,buyerEmail,ahippingAddress,deliveryMethod,subTotal);

            _unitOfWork.Repository<Order>().Add(order);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return null;

            await _basketRepo.DeleteBasketAcync(basketId);

            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecifications(id,buyerEmail);
            return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecifications(buyerEmail);
            return await _unitOfWork.Repository<Order>().ListAsync(spec);
        }
    }
}