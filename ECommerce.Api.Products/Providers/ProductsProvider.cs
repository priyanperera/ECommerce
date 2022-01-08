using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Interfaces;
using ECommerce.Api.Products.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Products.Providers
{
    public class ProductsProvider : IProductsProvider
    {
        private readonly ProductsDbContext productsDbContext;
        private readonly ILogger<ProductsProvider> logger;
        private readonly IMapper mapper;

        public ProductsProvider(ProductsDbContext productsDbContext, ILogger<ProductsProvider> logger, IMapper mapper)
        {
            this.productsDbContext = productsDbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (!productsDbContext.Products.Any())
            {
                productsDbContext.Products.Add(new Product { Id = 1, Name = "Bats", Inventory = 10, Price = 300 });
                productsDbContext.Products.Add(new Product { Id = 2, Name = "Balls", Inventory = 20, Price = 200 });
                productsDbContext.Products.Add(new Product { Id = 3, Name = "Gloves", Inventory = 30, Price = 100 });
                productsDbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<ProductDto> Products, string ErrorMessage)> GetProductsAsync()
        {
            try
            {
                var prodcuts = await productsDbContext.Products.ToListAsync();
                if (prodcuts != null && prodcuts.Any())
                {
                    var result = mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(prodcuts);
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

        public async Task<(bool IsSuccess, ProductDto Product, string ErrorMessage)> GetProductAsync(int id)
        {
            try
            {
                var prodcut = await productsDbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
                if (prodcut != null)
                {
                    var result = mapper.Map<Product, ProductDto>(prodcut);
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
