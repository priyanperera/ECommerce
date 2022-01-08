using AutoMapper;
using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Interfaces;
using ECommerce.Api.Customers.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Providers
{
    public class CustomersProvider : ICustomersProvider
    {
        private readonly CustomersDbContext customersDbContext;
        private readonly ILogger<CustomersProvider> logger;
        private readonly IMapper mapper;

        public CustomersProvider(CustomersDbContext customersDbContext, ILogger<CustomersProvider> logger, IMapper mapper)
        {
            this.customersDbContext = customersDbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (!customersDbContext.Customers.Any())
            {
                customersDbContext.Customers.Add(new Customer { Id = 1, Name = "Priyan", Address = "Epping" });
                customersDbContext.Customers.Add(new Customer { Id = 2, Name = "Nadeeka", Address = "Epping" });
                customersDbContext.Customers.Add(new Customer { Id = 3, Name = "Miron", Address = "Epping" });
                customersDbContext.Customers.Add(new Customer { Id = 4, Name = "Tiana", Address = "Epping" });
                customersDbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, CustomerDto Customer, string ErrorMessage)> GetCustomerAsync(int id)
        {
            try
            {
                var customer = await customersDbContext.Customers.FirstOrDefaultAsync(x => x.Id == id);
                if (customer != null)
                {
                    var result = mapper.Map<Customer, CustomerDto>(customer);
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

        public async Task<(bool IsSuccess, IEnumerable<CustomerDto> Customers, string ErrorMessage)> GetCustomersAsync()
        {
            try
            {
                var customers = await customersDbContext.Customers.ToListAsync();
                if (customers != null && customers.Any())
                {
                    var result = mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerDto>>(customers);
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
