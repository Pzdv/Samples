using Grpc.Net.Client;
using NorthwindDAL.Enum;
using NorthwindDAL.Model;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace gRPC.Tests
{
    public class Tests
    {
        private NorthwindRPC.NorthwindRPCClient _client;
        [SetUp]
        public void Setup()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            _client = new NorthwindRPC.NorthwindRPCClient(channel);
        }

        [Test]
        public void GetOrder()
        {
            Order expectedOrder;

            using (var db = new NorthwindContext())
            {
                expectedOrder = db.Orders.FirstOrDefault();
            }

          
            var reply = _client.GetOrder(new GetOrderRequest() { Id = expectedOrder.OrderId });
            var actualOrder = JsonSerializer.Deserialize<Order>(reply.Order);

            Assert.AreEqual(expectedOrder.OrderId, actualOrder.OrderId);        
        }

        [Test]
        public void GetOrders()
        {
            List<Order> expectedOrders;
            using (var db = new NorthwindContext())
            {
                expectedOrders = db.Orders.OrderBy(o => o.OrderId).ToList();
            }

            var reply = _client.GetOrders(new GetOrdersRequest());
            var actualOrder = JsonSerializer.Deserialize<List<Order>>(reply.Orders).OrderBy(o => o.OrderId).ToList();


            for (int i = 0; i < expectedOrders.Count; i++)
            {
                Assert.AreEqual(expectedOrders[i].OrderId, actualOrder[i].OrderId);
            }
        }

        [Test]
        public void Create()
        {
            var creatingOrder = new Order
            {
                EmployeeId = 2,
                CustomerId = "ANTON"
            };
            var creatingOrderJson = JsonSerializer.Serialize(creatingOrder);

            var createdOrderId = _client.CreateOrder(new CreateOrderRequest() { OrderJson = creatingOrderJson }).CreatingResult;

            Order orderFromDb;
            using (var db = new NorthwindContext())
            {
                orderFromDb = db.Orders.FirstOrDefault(o => o.OrderId == createdOrderId);
            }
            Assert.IsNotNull(orderFromDb);
            Assert.AreEqual(orderFromDb.OrderId, createdOrderId);
        }

        [Test]
        public void Update()
        {
            Order existOrder;
            Customer customer;
            using (var db = new NorthwindContext())
            {
                var orders = db.Orders.ToList();
                existOrder = orders.FirstOrDefault(o => o.State == OrderState.New);
                customer = db.Customers.FirstOrDefault(c => c.CustomerId != existOrder.CustomerId);
            }
            existOrder.CustomerId = customer.CustomerId;

            var updatedOrderJson = JsonSerializer.Serialize(existOrder);
            _client.UpdateOrder(new UpdateOrderRequest() { OrderJson = updatedOrderJson });

            Order actualOrder;
            using (var db = new NorthwindContext())
            {
                actualOrder = db.Orders.FirstOrDefault(o => o.OrderId == existOrder.OrderId);
            }
            Assert.AreEqual(customer.CustomerId, actualOrder.CustomerId);
        }
    }
}