using hwEF.Model;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;

namespace EF.Tests
{
    public class Task2
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void OrdersQuery()
        {
            using (var dbContext = new NorthwindContext())
            {
                var orders = dbContext.Orders
                    .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                    .ThenInclude(p => p.Category);

                foreach (var order in orders)
                {
                    System.Console.WriteLine($"OrderID: {order.OrderId}");

                    foreach (var details in order.OrderDetails)
                    {
                        var productId = details.ProductId;
                        var productName = details.Product.ProductName;
                        var categoryName = details.Product.Category == null ? "Категория не указана" : details.Product.Category.CategoryName;

                        System.Console.WriteLine($"ProductID: {productId} ProductName: {productName} Category: {categoryName}");
                    }
                }
            }
        }

        [Test]
        public void BeveragesProductQuery()
        {
            using (var dbContext = new NorthwindContext())
            {
                var beveragesProducts = dbContext.Products
                    .Include(p => p.Category)
                    .Where(p => p.Category != null && p.Category.CategoryName == "Beverages");

                foreach (var product in beveragesProducts)
                {
                    System.Console.WriteLine($"Category: {product.Category.CategoryName} ProductName: {product.ProductName}");
                }
            }
        }

        [Test]
        public void CustomersQuery()
        {
            using (var dbContext = new NorthwindContext())
            {
                var customers = dbContext.Customers
                    .Include(c => c.Orders)
                    .Select(c => new { c.CompanyName, Order = c.Orders.OrderBy(o => o.OrderDate).FirstOrDefault() });

                foreach (var customer in customers)
                {
                    var firstOrderDate = string.Empty;
                    if (customer.Order == null || customer.Order.OrderDate == null)
                    {
                        firstOrderDate = "Клиент еще не совершал заказов";
                    }
                    else
                    {
                        firstOrderDate = customer.Order.OrderDate.ToString();
                    }

                    System.Console.WriteLine($"Cpmany: {customer.CompanyName} FirstOrder: {firstOrderDate}");
                }
            }
        }

        [Test]
        public void ActivityByMoth()
        {
            using (var dbContext = new NorthwindContext())
            {
                var activityByMonth = dbContext.Orders
                    .Include(o => o.Customer)
                    .Where(o => o.OrderDate.HasValue)
                    .GroupBy(o => o.OrderDate.Value.Month)
                    .Select(g => new { Moth = g.Key, Count = g.Count() })
                    .OrderBy(x => x.Moth);

                foreach (var activity in activityByMonth)
                {
                    System.Console.WriteLine($"Month: {activity.Moth} Count: {activity.Count}");
                }
            }
        }

        [Test]
        public void ActivityByYear()
        {
            using (var dbContext = new NorthwindContext())
            {
                var activityByYear = dbContext.Orders
                    .Include(o => o.Customer)
                    .Where(o => o.OrderDate.HasValue)
                    .GroupBy(o => o.OrderDate.Value.Year)
                    .Select(g => new { Year = g.Key, Count = g.Count() })
                    .OrderBy(x => x.Year);

                foreach (var activity in activityByYear)
                {
                    System.Console.WriteLine($"Month: {activity.Year} Count: {activity.Count}");
                }
            }
        }


        [Test]
        public void ActivityByMonthAndYear()
        {
            using (var dbContext = new NorthwindContext())
            {
                var activityByMonthAndYear = dbContext.Orders
                    .Include(o => o.Customer)
                    .Where(o => o.OrderDate.HasValue)
                    .GroupBy(o => new { o.OrderDate.Value.Year, o.OrderDate.Value.Month })
                    .Select(g => new { Date = g.Key, Count = g.Count() })
                    .OrderBy(x => x.Date.Year)
                    .ThenBy(x => x.Date.Month);

                foreach (var activity in activityByMonthAndYear)
                {
                    System.Console.WriteLine($"Year: {activity.Date.Year} Month: {activity.Date.Month} Count: {activity.Count}");
                }
            }
        }
    }
}