using Northwind.Context.Interfaces;
using Northwind.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Northwind.DAL.Context
{
    public class DbContext : IDbContext
    {
        private readonly string _connectionString;

        public DbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<OrderDetails> GetOrderDetails(Order order)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("GetOrderDetails", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrderID", order.OrderID);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var orderDetails = new OrderDetails()
                            {
                                OrderID = ConvertFromDBVal<int>(reader["OrderID"]),
                                ProductID = ConvertFromDBVal<int>(reader["ProductID"]),
                                UnitPrice = ConvertFromDBVal<decimal>(reader["UnitPrice"]),
                                Quantity = ConvertFromDBVal<Int16>(reader["Quantity"]),
                                Discount = ConvertFromDBVal<Single>(reader["Discount"]),
                                ProductName = ConvertFromDBVal<string>(reader["ProductName"])
                            };
                            yield return orderDetails;
                        }
                    }
                }
            }
        }

        public IEnumerable<Order> GetOrders()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("GetOrders", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var order = new Order()
                            {
                                OrderID = ConvertFromDBVal<int>(reader["OrderID"]),
                                CustomerID = ConvertFromDBVal<string>(reader["CustomerID"]),
                                EmployeeID = ConvertFromDBVal<int>(reader["EmployeeID"]),
                                RequiredDate = ConvertFromDBVal<DateTime?>(reader["RequiredDate"]),
                                ShipVia = ConvertFromDBVal<int>(reader["ShipVia"]),
                                Freight = ConvertFromDBVal<decimal>(reader["Freight"]),
                                ShipName = ConvertFromDBVal<string>(reader["ShipName"]),
                                ShipAddress = ConvertFromDBVal<string>(reader["ShipAddress"]),
                                ShipCity = ConvertFromDBVal<string>(reader["ShipCity"]),
                                ShipRegion = ConvertFromDBVal<string>(reader["ShipRegion"]),
                                ShipPostalCode = ConvertFromDBVal<string>(reader["ShipPostalCode"]),
                                ShipCountry = ConvertFromDBVal<string>(reader["ShipCountry"])
                            };
                            var orderDate = ConvertFromDBVal<DateTime?>(reader["OrderDate"]);
                            var shippedDate = ConvertFromDBVal<DateTime?>(reader["ShippedDate"]);
                            order.SetOrderDate(orderDate);
                            order.SetShippedDate(shippedDate);

                            yield return order;
                        }
                    }
                }
            }
        }

        public void Create(OrderInfo orderInfo)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var newOrderId = CreateOrder(orderInfo.Order, connection);

                foreach (var item in orderInfo.OrderDetails)
                {
                    item.OrderID = newOrderId;
                }

                CreateOrderDetails(orderInfo.OrderDetails, connection);
            }
        }

        private void CreateOrderDetails(IEnumerable<OrderDetails> orderDetails, SqlConnection connection)
        {
            using (var command = new SqlCommand("CreateNewOrderDetails", connection))
            {
                foreach (var e in orderDetails)
                {
                    command.Parameters.AddWithValue("@OrderID", e.OrderID);
                    command.Parameters.AddWithValue("@ProductID", e.ProductID);
                    command.Parameters.AddWithValue("@UnitPrice", e.UnitPrice);
                    command.Parameters.AddWithValue("@Quantity", e.Quantity);
                    command.Parameters.AddWithValue("@Discount", e.Discount);

                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.ExecuteNonQuery();
                }
            }
        }

        private int CreateOrder(Order order, SqlConnection connection)
        {
            var orderId = -1m;
            using (var command = new SqlCommand("CreateNewOrder", connection))
            {
                command.Parameters.AddWithValue("@CustomerID", order.CustomerID);
                command.Parameters.AddWithValue("@EmployeeID", order.EmployeeID);
                command.Parameters.AddWithValue("@RequiredDate", order.RequiredDate);
                command.Parameters.AddWithValue("@ShipVia", order.ShipVia);
                command.Parameters.AddWithValue("@Freight", order.Freight);
                command.Parameters.AddWithValue("@ShipName", order.ShipName);
                command.Parameters.AddWithValue("@ShipAddress", order.ShipAddress);
                command.Parameters.AddWithValue("@ShipCity", order.ShipCity);
                command.Parameters.AddWithValue("@ShipRegion", order.ShipRegion);
                command.Parameters.AddWithValue("@ShipPostalCode", order.ShipPostalCode);
                command.Parameters.AddWithValue("@ShipCountry", order.ShipCountry);

                command.CommandType = System.Data.CommandType.StoredProcedure;
                orderId = ConvertFromDBVal<decimal>(command.ExecuteScalar());
            }
            return (int)orderId;
        }

        public void Update(Order order)
        {
            var updatedRowCount = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("UpdateOrder", connection))
                {
                    command.Parameters.AddWithValue("@OrderID", order.OrderID);
                    command.Parameters.AddWithValue("@CustomerID", order.CustomerID);
                    command.Parameters.AddWithValue("@EmployeeID", order.EmployeeID);
                    command.Parameters.AddWithValue("@RequiredDate", order.RequiredDate);
                    command.Parameters.AddWithValue("@ShipVia", order.ShipVia);
                    command.Parameters.AddWithValue("@Freight", order.Freight);
                    command.Parameters.AddWithValue("@ShipName", order.ShipName);
                    command.Parameters.AddWithValue("@ShipAddress", order.ShipAddress);
                    command.Parameters.AddWithValue("@ShipCity", order.ShipCity);
                    command.Parameters.AddWithValue("@ShipRegion", order.ShipRegion);
                    command.Parameters.AddWithValue("@ShipPostalCode", order.ShipPostalCode);
                    command.Parameters.AddWithValue("@ShipCountry", order.ShipCountry);

                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    updatedRowCount = ConvertFromDBVal<int>(command.ExecuteScalar());
                }
            }

            if(updatedRowCount < 1)
            {
                throw new InvalidOperationException(message: $"В базе данных нет заказов с OrderID: {order.OrderID}");
            }
        }

        public void Delete(Order order)
        {
            var deletedRows = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("DeleteOrder", connection))
                {
                    command.Parameters.AddWithValue("OrderID", order.OrderID);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    deletedRows = ConvertFromDBVal<int>(command.ExecuteScalar());
                }
            }

            if(deletedRows < 1)
            {
                throw new InvalidOperationException(message: $"В базе данных нет заказов с OrderID: {order.OrderID}");
            }
        }

        public void SetShippedDate(Order order, DateTime shippedDate)
        {
            var updatedRows = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SetShippedDate", connection))
                {
                    command.Parameters.AddWithValue("@ShippedDate", shippedDate);
                    command.Parameters.AddWithValue("@OrderID", order.OrderID);

                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    updatedRows =  ConvertFromDBVal<int>(command.ExecuteScalar());
                }
            }

            if(updatedRows < 1)
            {
                throw new InvalidOperationException(message: $"В базе данных нет заказов с OrderID: {order.OrderID}");
            }
        }

        public void SetOrderDate(Order order, DateTime orderDate)
        {
            var updatedRows = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SetOrderDate", connection))
                {
                    command.Parameters.AddWithValue("@OrderDate", orderDate);
                    command.Parameters.AddWithValue("@OrderID", order.OrderID);

                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    updatedRows = ConvertFromDBVal<int>(command.ExecuteScalar());
                }
            }

            if (updatedRows < 1)
            {
                throw new InvalidOperationException(message: $"В базе данных нет заказов с OrderID: {order.OrderID}");
            }
        }

        public IEnumerable<CustomerOrderHistory> CustomerOrdersHistory(Customer customer)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("CustOrderHist", connection))
                {
                    command.Parameters.AddWithValue("@CustomerID", customer.CustomerID);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var customerOrderHistory = new CustomerOrderHistory()
                            {
                                ProductName = ConvertFromDBVal<string>(reader["ProductName"]),
                                Total = ConvertFromDBVal<int>(reader["Total"])
                            };

                            yield return customerOrderHistory;
                        }
                    }
                }
            }
        }

        public IEnumerable<CustomerOrdersDetail> CustomerOrdersDetail(Order order)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("CustOrdersDetail", connection))
                {
                    command.Parameters.AddWithValue("@OrderID", order.OrderID);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var orderDetail = new CustomerOrdersDetail()
                            {
                                ProductName = ConvertFromDBVal<string>(reader["ProductName"]),
                                UnitPrice = ConvertFromDBVal<decimal>(reader["UnitPrice"]),
                                Quantity = ConvertFromDBVal<Int16>(reader["Quantity"]),
                                Discount = ConvertFromDBVal<int>(reader["Discount"]),
                                ExtendedPrice = ConvertFromDBVal<decimal>(reader["ExtendedPrice"])
                            };
                            yield return orderDetail;
                        }
                    }
                }
            }
        }

        private static T ConvertFromDBVal<T>(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return default;
            }
            else
            {
                return (T)obj;
            }
        }

        public void DeleteFirst78byteFromPicture()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("select * from categories"))
                {
                    command.Connection = connection;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var categoryID = (int)reader.GetValue(0);
                            var currentImage = (byte[])reader.GetValue(3);

                            var newImage = DeleteFirst78Bytes(currentImage);
                            WriteNewImage(newImage, categoryID);
                        }
                    }
                }
            }
        }

        private void WriteNewImage(byte[] imageBytes, int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("update categories set Picture = @imageBytes where CategoryID = @id"))
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.Parameters.AddWithValue("@imageBytes", imageBytes);
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
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
