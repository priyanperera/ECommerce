﻿using ECommerce.Api.Customers.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Interfaces
{
    public interface ICustomersProvider
    {
        Task<(bool IsSuccess, IEnumerable<CustomerDto> Customers, string ErrorMessage)> GetCustomersAsync();
        Task<(bool IsSuccess, CustomerDto Customer, string ErrorMessage)> GetCustomerAsync(int id);
    }
}
