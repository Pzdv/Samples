using Dapper;
using Northwind.Context.Interfaces;
using Northwind.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Northwind.Context
{
    public class DapperDbContext : IDbContext
    {
        private readonly string _connectionString;

        public DapperDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Create(OrderInfo orderInfo)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var orderId = CreateNewOrder(connection, orderInfo.Order);
                CreateNewOrderDetails(connection, orderInfo.OrderDetails, orderId);
            }
        }

        private static int CreateNewOrder(IDbConnection connection, Order order)
        {
            var parameters = new
            {
                order.CustomerID,
                order.EmployeeID,
                order.RequiredDate,
                order.ShipVia,
                order.Freight,
                order.ShipName,
                order.ShipAddress,
                order.ShipCity,
                order.ShipRegion,
                order.ShipPostalCode,
                order.ShipCountry
            };

            var orderId = connection.ExecuteScalar<int>("CreateNewOrder", parameters, commandType: CommandType.StoredProcedure);
            return orderId;
        }

        private static void CreateNewOrderDetails(IDbConnection connection, IEnumerable<OrderDetails> orderDetails, int orderId)
        {
            foreach (var od in orderDetails)
            {
                od.OrderID = orderId;
                var parameters = new
                {
                    od.OrderID,
                    od.ProductID,
                    od.UnitPrice,
                    od.Quantity,
                    od.Discount
                };
                connection.Execute("CreateNewOrderDetails", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<CustomerOrdersDetail> CustomerOrdersDetail(Order order)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new
                {
                    order.OrderID
                };
                var result = connection.Query<CustomerOrdersDetail>("CustOrdersDetail", parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public IEnumerable<CustomerOrderHistory> CustomerOrdersHistory(Customer customer)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new
                {
                    customer.CustomerID
                };
                var result = connection.Query<CustomerOrderHistory>("CustOrderHist", parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public void Delete(Order order)
        {
            var deletedRows = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new
                {
                    order.OrderID
                };
                deletedRows = connection.ExecuteScalar<int>("DeleteOrder", parameters, commandType: CommandType.StoredProcedure);

                if(deletedRows < 1)
                {
                    throw new InvalidOperationException(message: $"В базе данных нет заказов с OrderID: {order.OrderID}");
                }
            }

            if (deletedRows < 1)
            {
                throw new InvalidOperationException(message: $"В базе данных нет заказов с OrderID: {order.OrderID}");
            }
        }

        public IEnumerable<OrderDetails> GetOrderDetails(Order order)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new
                {
                    order.OrderID
                };
                var orderDetails = connection.Query<OrderDetails>("GetOrderDetails", parameters, commandType: CommandType.StoredProcedure);
                return orderDetails;
            }
        }

        public IEnumerable<Order> GetOrders()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var orders = connection.Query<Order>("GetOrders", CommandType.StoredProcedure);
                foreach (var order in orders)
                {
                    order.SetOrderDate(order.OrderDate);
                    order.SetShippedDate(order.ShippedDate);
                    yield return order;
                }
            }
        }

        public void SetOrderDate(Order order, DateTime orderDate)
        {
            var updatedRows = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new
                {
                    order.OrderID,
                    OrderDate = orderDate
                };
                updatedRows = connection.ExecuteScalar<int>("SetOrderDate", parameters, commandType: CommandType.StoredProcedure);
            }

            if (updatedRows < 1)
            {
                throw new InvalidOperationException(message: $"В базе данных нет заказов с OrderID: {order.OrderID}");
            }
        }

        public void SetShippedDate(Order order, DateTime shippedDate)
        {
            var updateRows = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new
                {
                    order.OrderID,
                    ShippedDate = shippedDate
                };
                updateRows = connection.ExecuteScalar<int>("SetShippedDate", parameters, commandType: CommandType.StoredProcedure);
            }

            if(updateRows < 1)
            {
                throw new InvalidOperationException(message: $"В базе данных нет заказов с OrderID: {order.OrderID}");
            }
        }

        public void Update(Order order)
        {
            var updatedRowCount = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new
                {
                    order.OrderID,
                    order.CustomerID,
                    order.EmployeeID,
                    order.RequiredDate,
                    order.ShipVia,
                    order.Freight,
                    order.ShipName,
                    order.ShipAddress,
                    order.ShipCity,
                    order.ShipRegion,
                    order.ShipPostalCode,
                    order.ShipCountry
                };

                updatedRowCount = connection.ExecuteScalar<int>("UpdateOrder", parameters, commandType: CommandType.StoredProcedure);
            }

            if (updatedRowCount < 1)
            {
                throw new InvalidOperationException(message: $"В базе данных нет заказов с OrderID: {order.OrderID}");
            }
        }

        public void DeleteFirst78byteFromPicture()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var result = connection.Query("select * from categories", commandType: CommandType.Text);

                foreach (var row in result)
                {
                    var newImage = DeleteFirst78Bytes(row.Picture);
                    WriteNewImage(newImage, row.CategoryID);
                }
            }
        }

        private void WriteNewImage(byte[] imageBytes, int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new
                {
                    imageBytes,
                    id
                };
                connection.Execute("update categories set Picture = @imageBytes where CategoryID = @id", parameters);
            }
        }

        private byte[] DeleteFirst78Bytes(byte[] currentImage)
        {
            var newImage = new byte[currentImage.Length - 78];
            for (var i = 78; i < currentImage.Length; i++)
            {
                newImage[i - 78] = currentImage[i];
            }
            return newImage;
        }
    }
}
