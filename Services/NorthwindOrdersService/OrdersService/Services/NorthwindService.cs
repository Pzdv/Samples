using System.Text.Json;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NorthwindDAL.Model;

namespace OrdersService
{
    public class NorthwindService : NorthwindRPC.NorthwindRPCBase
    {
        private readonly NorthwindContext _context;
        public NorthwindService(NorthwindContext context)
        {
            _context = context;
        }

        public override Task<GetOrdersReply> GetOrders(GetOrdersRequest request, ServerCallContext context)
        {
            var orders = _context.Orders.ToList();
            var ordersJson = JsonSerializer.Serialize(orders);
            var reply = new GetOrdersReply()
            {
                Orders = ordersJson
            };
            return Task.FromResult(reply);
        }

        public override Task<GetOrderReply> GetOrder(GetOrderRequest request, ServerCallContext context)
        {
            var order = _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od=>od.Product)
                .FirstOrDefault(o => o.OrderId == request.Id);
            if(order == null)
            {
                var answer = "Not Found";
                var replyNoFound = new GetOrderReply()
                {
                    Order = answer
                };
                return Task.FromResult(replyNoFound);
            }
            var orderJson = JsonSerializer.Serialize(order);
            var reply = new GetOrderReply()
            {
                Order = orderJson
            };
            return Task.FromResult(reply);
        }

        public override Task<CreateOrderReply> CreateOrder(CreateOrderRequest request, ServerCallContext context)
        {
            var order = DeserializeOrderFromRequest(request.OrderJson);

            try
            {
                _context.Orders.Add(order);
                _context.SaveChanges();

                var reply = new CreateOrderReply()
                {
                    CreatingResult = order.OrderId
                };
                return Task.FromResult(reply);
            }
            catch(Exception ex)
            {
                var status = new Status(StatusCode.Internal, ex.Message, ex);
                context.Status = status;
                throw new RpcException(status);
            }
        }

        public override Task<UpdateOrderReply> UpdateOrder(UpdateOrderRequest request, ServerCallContext context)
        {
            var inputOrder = DeserializeOrderFromRequest(request.OrderJson);

            var existOrder = _context.Orders.FirstOrDefault(o => o.OrderId == inputOrder.OrderId);

            if(existOrder == null)
            {
                var status = new Status(StatusCode.NotFound, $"Order with id = {inputOrder.OrderId} does not exist");
                throw new RpcException(status);
            }

            try
            {
                _context.Entry(existOrder).State = EntityState.Detached;

                inputOrder.OrderDate = existOrder.OrderDate;
                inputOrder.ShippedDate = existOrder.ShippedDate;

                _context.Entry(inputOrder).State = EntityState.Modified;

                _context.SaveChanges();

                var reply = new UpdateOrderReply()
                {
                    UpdatingResult = "Successful"
                };

                return Task.FromResult(reply);
            }
            catch (Exception ex)
            {
                var status = new Status(StatusCode.Internal, ex.Message, ex);
                throw new RpcException(status);
            }
        }

        public override Task<DeleteOrderReply> DeleteOrder(DeleteOrderRequest request, ServerCallContext context)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == request.Id);
            if(order == null)
            {
                var status = new Status(StatusCode.NotFound, $"Order with id = {request.Id} does not exist");
                throw new RpcException(status);
            }
            try
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
                var reply = new DeleteOrderReply()
                {
                    DeletingResult = "Successful"
                };
                return Task.FromResult(reply);
            }
            catch (Exception ex)
            {
                var status = new Status(StatusCode.Internal, ex.Message, ex);
                throw new RpcException(status);
            }
        }

        private static Order DeserializeOrderFromRequest(string orderJson)
        {
            Order order;
            try
            {
                order = JsonSerializer.Deserialize<Order>(orderJson);
                return order;
            }
            catch (Exception ex)
            {
                var status = new Status(StatusCode.InvalidArgument, ex.Message, ex);
                throw new RpcException(status);
            }
        }
    }
}