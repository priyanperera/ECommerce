using ECommerce.Api.Orders.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Api.Orders.Interfaces
{
    public interface IOrdersProvider
    {
        (bool IsSuccess, IEnumerable<OrderDto> Orders, string ErrorMessage) GetOrdersAsync(int customerId);
    }
}
