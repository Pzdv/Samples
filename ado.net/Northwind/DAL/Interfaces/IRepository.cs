using Northwind.Models;
using System;
using System.Collections.Generic;

namespace Northwind.DAL.Interfaces
{
    public interface IRepository
    {
        IEnumerable<Order> GetOrders();
        OrderInfo GetOrderInfo(Order order);
        void Create(OrderInfo order);
        void Update(Order order);
        void Delete(Order order);
        void SetOrderDate(Order order, DateTime orderDate);
        void SetShippedDate(Order order, DateTime shippedDate);
        IEnumerable<CustomerOrderHistory> CustomerOrdersHistory(Customer customer);
        IEnumerable<CustomerOrdersDetail> CustomerOrdersDetail(Order order);
        void DeleteFirst78byteFromPicture();
    }
}
