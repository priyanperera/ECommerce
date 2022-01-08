using ECommerce.Api.Search.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrderService orderService;
        private readonly IProductService productService;
        private readonly ICustomerService customerService;

        public SearchService(
            IOrderService orderService,
            IProductService productService,
            ICustomerService customerService)
        {
            this.orderService = orderService;
            this.productService = productService;
            this.customerService = customerService;
        }

        public async Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int customerId)
        {
            var orders = await orderService.GetOrdersAsync(customerId);
            var products = await productService.GetProductsAsync();

            if (orders.IsSuccess)
            {
                foreach (var order in orders.Orders)
                {
                    var customer = await customerService.GetCustomerAsync(customerId);
                    order.CustomerName = customer.IsSuccess ? customer.Customer.Name : "Customer name not available";

                    foreach (var item in order.Items)
                    {
                        item.ProductName = products.IsSuccess ?
                            products.Products.FirstOrDefault(x => x.Id == item.ProductId)?.Name :
                            "Product information not available";
                    }
                }
                var result = new
                {
                    Orders = orders.Orders
                };
                return (orders.IsSuccess, result);
            }
            return (false, null);
        }
    }
}
