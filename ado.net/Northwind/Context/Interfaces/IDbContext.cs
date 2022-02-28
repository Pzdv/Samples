using Northwind.Models;
using System;
using System.Collections.Generic;

namespace Northwind.Context.Interfaces
{
    public interface IDbContext
    {
        IEnumerable<Order> GetOrders();
        IEnumerable<OrderDetails> GetOrderDetails(Order order);
        void Create(OrderInfo order);
        void Update(Order order);
        void Delete(Order order);
        void SetShippedDate(Order order, DateTime shippedDate);
        void SetOrderDate(Order order, DateTime orderDate);
        IEnumerable<CustomerOrderHistory> CustomerOrdersHistory(Customer customer);
        IEnumerable<CustomerOrdersDetail> CustomerOrdersDetail(Order order);
        void DeleteFirst78byteFromPicture();
    }
}