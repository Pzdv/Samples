using Northwind.Context.Interfaces;
using Northwind.DAL.Interfaces;
using Northwind.Models;
using System;
using System.Collections.Generic;

namespace Northwind.DAL
{
    public class NorthwindSqlRepository : IRepository
    {
        private readonly IDbContext _dbContext;

        public NorthwindSqlRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Order> GetOrders()
        {
            return _dbContext.GetOrders();
        }

        public void Create(OrderInfo order)
        {
            _dbContext.Create(order);
        }

        public void Update(Order order)
        {
            _dbContext.Update(order);
        }

        public void Delete(Order order)
        {
            _dbContext.Delete(order);
        }

        public OrderInfo GetOrderInfo(Order order)
        {
            var orderInfo = new OrderInfo
            {
                OrderDetails = _dbContext.GetOrderDetails(order),
                Order = order
            };
            return orderInfo;
        }

        public void SetOrderDate(Order order, DateTime orderDate)
        {
            _dbContext.SetOrderDate(order, orderDate);
        }

        public void SetShippedDate(Order order, DateTime shippedDate)
        {
            _dbContext.SetShippedDate(order, shippedDate);
        }

        public IEnumerable<CustomerOrderHistory> CustomerOrdersHistory(Customer customer)
        {
            return _dbContext.CustomerOrdersHistory(customer);
        }

        public IEnumerable<CustomerOrdersDetail> CustomerOrdersDetail(Order order)
        {
            return _dbContext.CustomerOrdersDetail(order);
        }
        public void DeleteFirst78byteFromPicture()
        {
            _dbContext.DeleteFirst78byteFromPicture();
        }
    }
}
