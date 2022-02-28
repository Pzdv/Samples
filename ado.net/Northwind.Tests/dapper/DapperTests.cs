using Northwind.Context;
using Northwind.Context.Interfaces;
using Northwind.DAL;
using Northwind.DAL.Interfaces;
using Northwind.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Tests.dapper
{
    class DapperTests
    {
        private IDbContext dbContext;
        private IRepository repository;
        private BLL.Northwind northwind;
        private string connectionString;

        [SetUp]
        public void Setup()
        {
            connectionString = "Server=localhost;Database=northwind;Trusted_Connection=True;";
            dbContext = new DapperDbContext(connectionString);
            repository = new NorthwindSqlRepository(dbContext);
            northwind = new BLL.Northwind(repository);
        }

        [Test]
        public void GetOrders()
        {
            var orders = northwind.GetOrders();

            Assert.That(orders != null);
        }

        [Test]
        public void CreateNewOrder()
        {
            var nextOrderID = 0m;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT IDENT_CURRENT('Orders')"))
                {
                    command.Connection = connection;
                    nextOrderID = (decimal)command.ExecuteScalar() + 1;
                }
            }

            var order = new Order()
            {
                CustomerID = "ANTON",
                EmployeeID = 2,
                ShipVia = 3
            };
            var orderDetails = new OrderDetails()
            {
                ProductID = 7,
                Quantity = 2,
                Discount = 0,
                UnitPrice = 15.00m
            };

            var orderInfo = new OrderInfo()
            {
                Order = order,
                OrderDetails = new List<OrderDetails> { orderDetails }
            };

            northwind.CreateNewOrder(orderInfo);

            var actualLastOrder = northwind.GetOrders().OrderByDescending(x => x.OrderID).FirstOrDefault();

            Assert.AreEqual(nextOrderID, actualLastOrder.OrderID);
        }

        [Test]
        public void CustomerOrdersDetail()
        {
            var order = northwind.GetOrders().Where(x => x.OrderID == 10378).FirstOrDefault();
            var customerOrdersDetail = northwind.CustomerOrdersDetail(order);

            Assert.That(customerOrdersDetail != null);
        }

        [Test]
        public void CustomerOrderHistory()
        {
            var customer = new Customer()
            {
                CustomerID = "ANTON"
            };

            var customerOrderHistory = northwind.CustomerOrdersHistory(customer);

            Assert.That(customerOrderHistory != null);
        }

        [Test]
        public void DeleteOrder()
        {
            var currentLastOrder = northwind.GetOrders().OrderByDescending(x => x.OrderID).FirstOrDefault();
            var expectedLastOrder = northwind.GetOrders().OrderByDescending(x => x.OrderID).Skip(1).FirstOrDefault();

            northwind.DeleteOrder(currentLastOrder);
            var actualLastOrderID = northwind.GetOrders().OrderByDescending(x => x.OrderID).FirstOrDefault();

            Assert.AreEqual(expectedLastOrder.OrderID, actualLastOrderID.OrderID);
        }

        [Test]
        public void GetOrderInfo()
        {
            var order = northwind.GetOrders().Where(x => x.OrderID == 10378).FirstOrDefault();
            var orderInfo = northwind.GetOrderInfo(order);

            Assert.That(orderInfo != null);
        }

        [Test]
        public void UpdateOrder()
        {
            #region CreateNewOrder
            var order = new Order()
            {
                CustomerID = "ANTON",
                EmployeeID = 2,
                ShipVia = 3
            };

            var orderDetails = new OrderDetails()
            {
                ProductID = 10,
                UnitPrice = 100,
                Quantity = 1,
                Discount = 0
            };

            var orderInfo = new OrderInfo()
            {
                Order = order,
                OrderDetails = new List<OrderDetails> { orderDetails }
            };

            northwind.CreateNewOrder(orderInfo);
            #endregion

            order = northwind.GetOrders().OrderByDescending(x => x.OrderID).FirstOrDefault();

            #region SetNewValues
            var newRequiredDate = System.DateTime.Now.AddDays(20).Date;
            var newCustomerID = "RATTC";
            var newEmployeeID = 8;
            var newShipVia = 2;
            var newFreight = 36;
            var newShipName = "Bon app'";
            var newShipAddress = "12, rue des Bouchers";
            var newShipCity = "Marseille";
            var newShipRegion = "NM";
            var newShipPostalCode = "13008";
            var newShipCountry = "France";

            order.CustomerID = newCustomerID;
            order.RequiredDate = newRequiredDate;
            order.EmployeeID = newEmployeeID;
            order.ShipVia = newShipVia;
            order.Freight = newFreight;
            order.ShipAddress = newShipAddress;
            order.ShipName = newShipName;
            order.ShipCity = newShipCity;
            order.ShipRegion = newShipRegion;
            order.ShipPostalCode = newShipPostalCode;
            order.ShipCountry = newShipCountry;
            #endregion

            northwind.UpdateOrder(order);
            var updateOrder = northwind.GetOrders().OrderByDescending(x => x.OrderID).FirstOrDefault();

            #region Asserts
            Assert.AreEqual(newRequiredDate, updateOrder.RequiredDate);
            Assert.AreEqual(newCustomerID, updateOrder.CustomerID);
            Assert.AreEqual(newEmployeeID, updateOrder.EmployeeID);
            Assert.AreEqual(newShipVia, updateOrder.ShipVia);
            Assert.AreEqual(newFreight, updateOrder.Freight);
            Assert.AreEqual(newShipAddress, updateOrder.ShipAddress);
            Assert.AreEqual(newShipName, updateOrder.ShipName);
            Assert.AreEqual(newShipCity, updateOrder.ShipCity);
            Assert.AreEqual(newShipRegion, updateOrder.ShipRegion);
            Assert.AreEqual(newShipPostalCode, updateOrder.ShipPostalCode);
            Assert.AreEqual(newShipCountry, updateOrder.ShipCountry);
            #endregion
        }

        [Test]
        public void SetOrderStateInWork()
        {
            var expectedOrderDate = DateTime.Now.Date;

            #region CreateNewOrder
            var order = new Order()
            {
                CustomerID = "ANTON",
                EmployeeID = 2,
                ShipVia = 3
            };

            var orderDetails = new OrderDetails()
            {
                ProductID = 10,
                UnitPrice = 100,
                Quantity = 1,
                Discount = 0
            };

            var orderInfo = new OrderInfo()
            {
                Order = order,
                OrderDetails = new List<OrderDetails> { orderDetails }
            };

            northwind.CreateNewOrder(orderInfo);
            #endregion

            var createdOrder = northwind.GetOrders().OrderByDescending(x => x.OrderID).FirstOrDefault();

            northwind.SetOrderStateInWork(createdOrder, expectedOrderDate);

            var updatedOrder = northwind.GetOrders().OrderByDescending(x => x.OrderID).FirstOrDefault();

            Assert.AreEqual(expectedOrderDate, updatedOrder.OrderDate);
            Assert.That(updatedOrder.State == Enums.OrderState.InWork);
        }

        [Test]
        public void SetOrderStatCompleted()
        {
            var expectedShippedDate = DateTime.Now.Date.AddDays(10);

            #region CreateNewOrder
            var order = new Order()
            {
                CustomerID = "ANTON",
                EmployeeID = 2,
                ShipVia = 3
            };

            var orderDetails = new OrderDetails()
            {
                ProductID = 10,
                UnitPrice = 100,
                Quantity = 1,
                Discount = 0
            };

            var orderInfo = new OrderInfo()
            {
                Order = order,
                OrderDetails = new List<OrderDetails> { orderDetails }
            };

            northwind.CreateNewOrder(orderInfo);
            #endregion

            var createdOrder = northwind.GetOrders().OrderByDescending(x => x.OrderID).FirstOrDefault();

            northwind.SetOrderStateCompleted(createdOrder, expectedShippedDate);

            var updatedOrder = northwind.GetOrders().OrderByDescending(x => x.OrderID).FirstOrDefault();

            Assert.AreEqual(expectedShippedDate, updatedOrder.ShippedDate);
            Assert.That(updatedOrder.State == Enums.OrderState.Completed);
        }
    }
}
