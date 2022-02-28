using Northwind.DAL.Interfaces;
using Northwind.Models;
using System;
using System.Collections.Generic;

namespace Northwind.BLL
{
    public class Northwind
    {
        private readonly IRepository _repository;

        public Northwind(IRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Order> GetOrders()
        {
            var orders = _repository.GetOrders();
            foreach (var order in orders)
            {
                yield return order;
            }
        }

        public OrderInfo GetOrderInfo(Order order)
        {
            var orderInfo = _repository.GetOrderInfo(order);
            return orderInfo;
        }

        public void CreateNewOrder(OrderInfo orderInfo)
        {
            _repository.Create(orderInfo);
        }

        public void UpdateOrder(Order order)
        {
            if (order.State == Enums.OrderState.New)
            {
                _repository.Update(order);
            }
            else
            {
                throw new InvalidOperationException(message: "Попытка изменить заказ в состоянии <В работе> или <Выполненный>");
            }
        }

        public void DeleteOrder(Order order)
        {
            if (order.State == Enums.OrderState.New)
            {
                _repository.Delete(order);
            }
            else
            {
                throw new InvalidOperationException(message: "Попытка удалить заказ в состоянии <В работе> или <Выполненный>");
            }
        }

        public void SetOrderStateInWork(Order order, DateTime orderDate)
        {
            order.SetOrderDate(orderDate);
            _repository.SetOrderDate(order, orderDate);
        }

        public void SetOrderStateCompleted(Order order, DateTime shippedDate)
        {
            order.SetShippedDate(shippedDate);
            _repository.SetShippedDate(order, shippedDate);
        }

        public IEnumerable<CustomerOrderHistory> CustomerOrdersHistory(Customer customer)
        {
            return _repository.CustomerOrdersHistory(customer);
        }

        public IEnumerable<CustomerOrdersDetail> CustomerOrdersDetail(Order order)
        {
            return _repository.CustomerOrdersDetail(order);
        }
        public void DeleteFirst78byteFromPicture()
        {
            _repository.DeleteFirst78byteFromPicture();
        }
    }
}
