using AutoMapper;
using ECommerce.Api.Orders.Db;
using ECommerce.Api.Orders.Interfaces;
using ECommerce.Api.Orders.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Orders.Providers
{
    public class OrdersProvider : IOrdersProvider
    {
        private readonly OrdersDbContext ordersDbContext;
        private readonly ILogger<OrdersProvider> logger;
        private readonly IMapper mapper;

        public OrdersProvider(OrdersDbContext ordersDbContext, ILogger<OrdersProvider> logger, IMapper mapper)
        {
            this.ordersDbContext = ordersDbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (!ordersDbContext.Orders.Any())
            {
                ordersDbContext.Orders.Add(new Order { CustomerId = 1, Id = 1, OrderDate = DateTime.Now, Total = 100, Items = new List<OrderItem> { new OrderItem { Id = 1, OrderId = 1, ProductId = 1, Quantity = 5, UnitPrice = 5 } } });
                ordersDbContext.Orders.Add(new Order { CustomerId = 2, Id = 2, OrderDate = DateTime.Now, Total = 100, Items = new List<OrderItem> { new OrderItem { Id = 2, OrderId = 2, ProductId = 1, Quantity = 5, UnitPrice = 5 } } });
                ordersDbContext.Orders.Add(new Order { CustomerId = 3, Id = 3, OrderDate = DateTime.Now, Total = 100, Items = new List<OrderItem> { new OrderItem { Id = 3, OrderId = 3, ProductId = 1, Quantity = 5, UnitPrice = 5 } } });
                ordersDbContext.Orders.Add(new Order { CustomerId = 4, Id = 4, OrderDate = DateTime.Now, Total = 100, Items = new List<OrderItem> { new OrderItem { Id = 4, OrderId = 4, ProductId = 1, Quantity = 5, UnitPrice = 5 } } });
                ordersDbContext.SaveChanges();
            }
        }

        public (bool IsSuccess, IEnumerable<OrderDto> Orders, string ErrorMessage) GetOrdersAsync(int customerId)
        {
            try
            {
                var orders = ordersDbContext.Orders.Where(x => x.CustomerId == customerId);
                if (orders != null && orders.Any())
                {
                    var result = mapper.Map<IEnumerable<Order>, IEnumerable<OrderDto>>(orders);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception e)
            {
                logger?.LogError(e.ToString());
                return (false, null, e.Message);
            }
        }
    }
}
