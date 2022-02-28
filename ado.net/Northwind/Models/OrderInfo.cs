using System.Collections.Generic;

namespace Northwind.Models
{
    public class OrderInfo
    {
        public IEnumerable<OrderDetails> OrderDetails { get; set; }
        public Order Order { get; set; }
    }
}