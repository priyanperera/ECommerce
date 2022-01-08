using System;
using System.Collections.Generic;

namespace ECommerce.Api.Search.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public int Total { get; set; }
        public IList<OrderItem> Items { get; set; }
    }
}
