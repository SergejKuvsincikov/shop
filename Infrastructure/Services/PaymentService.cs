using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAgregate;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Configuration;
using Stripe;
using Product = Core.Entities.Product;

namespace Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        IBasketRepository _basketRepository;
        IUnitOfWork _unitOfWork;
        IConfiguration _configuration;
        public PaymentService(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
            var basket = await _basketRepository.GetBasketAcync(basketId);
            var shippingPrice = 0m;

            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync((int)basket.DeliveryMethodId);
                shippingPrice = deliveryMethod.Price;
            }

            foreach(var item in basket.Items)
            {
                var productIem = await _unitOfWork.Repository<Product>().GetByIdAsync(int.Parse(item.Id));
                if (item.Price != productIem.Price)
                {
                    item.Price = productIem.Price;
                }
            }

            var service = new PaymentIntentService();
            PaymentIntent intent;

            if (string.IsNullOrEmpty(basket.PaimentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount =(long)  basket.Items.Sum(x => (x.Quantity * x.Price * 100)) + (long) shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() {"card"}
                };

                intent = await service.CreateAsync(options);
                basket.PaimentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount =(long)  basket.Items.Sum(x => (x.Quantity * x.Price * 100)) + (long) shippingPrice * 100
                };
                intent = await service.UpdateAsync(basket.PaimentIntentId,options);
            }

            await _basketRepository.UpdateBasketAcync(basket);
            return basket;
             

        }
    }
}