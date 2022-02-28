using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EF.Enum;
using hwEF.Model;

namespace EF.Tests
{
    public class Task3
    {
        [Test]
        public void AddNewOrder()
        {
            #region Before
            Console.WriteLine(Environment.NewLine + "До:");

            using (var dbContext = new NorthwindContext())
            {
                var orders = dbContext.Orders
                    .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                    .OrderByDescending(o => o.OrderId);
                foreach (var order in orders.Take(2))
                {
                    Console.WriteLine($"OrderID: {order.OrderId}");
                    foreach (var od in order.OrderDetails)
                    {
                        Console.WriteLine($"product: {od.ProductId}");
                    }
                }
            }
            #endregion

            var newOrder = new Order();
            var newOrderDetail1 = new OrderDetail();
            var newOrderDetail2 = new OrderDetail();

            using (var dbContext = new NorthwindContext())
            {
                var products = dbContext.Products;

                var product1 = products.FirstOrDefault();
                newOrderDetail1.Product = product1;
                newOrderDetail1.Quantity = 1;
                newOrderDetail1.Discount = 0;
                newOrderDetail1.UnitPrice = (decimal)product1.UnitPrice;

                var product2 = products.Skip(1).FirstOrDefault();
                newOrderDetail2.Product = product2;
                newOrderDetail2.Quantity = 1;
                newOrderDetail2.Discount = 0;
                newOrderDetail2.UnitPrice = (decimal)product2.UnitPrice;

                newOrder.OrderDetails.Add(newOrderDetail1);
                newOrder.OrderDetails.Add(newOrderDetail2);

                dbContext.Attach(newOrder);
                dbContext.Orders.Add(newOrder);
                dbContext.SaveChanges();
            }

            Console.WriteLine(Environment.NewLine + "После:");

            using (var dbContext = new NorthwindContext())
            {
                var orders = dbContext.Orders
                    .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                    .OrderByDescending(o => o.OrderId);
                foreach (var order in orders.Take(2))
                {
                    Console.WriteLine($"OrderID: {order.OrderId}");
                    foreach (var od in order.OrderDetails)
                    {
                        Console.WriteLine($"product: {od.ProductId}");
                    }
                }
            }
        }

        [Test]
        public void ChangeProductCategory()
        {
            using (var dbContext = new NorthwindContext())
            {

                var product = dbContext.Products.Include(p => p.Category).FirstOrDefault();
                Console.WriteLine($"Before: ProductID = {product.ProductId} ProductNname = {product.ProductName} Category = {product.Category.CategoryName}");

                var category = dbContext.Categories.Where(c => c.CategoryName != product.Category.CategoryName).FirstOrDefault();
                product.Category = category;
                dbContext.SaveChanges();
                product = dbContext.Products.Include(p => p.Category).FirstOrDefault();
                Console.WriteLine($"After: ProductID = {product.ProductId} ProductNname = {product.ProductName} Category = {product.Category.CategoryName}");
            }
        }

        [Test]
        public void AddProductList()
        {
            using (var dbContext = new NorthwindContext())
            {
                #region Before
                Console.WriteLine($"{Environment.NewLine}Список категорий до добавления продукта с новой категорией:");
                var currentCategories = dbContext.Categories;
                foreach (var category in currentCategories)
                {
                    Console.WriteLine($"{category.CategoryName}");
                }

                Console.WriteLine($"{Environment.NewLine}Список последних 10 продуктов до добавления:");
                var products = dbContext.Products.OrderByDescending(p => p.ProductId).Take(10);
                foreach (var product in products)
                {
                    Console.WriteLine($"{product.ProductName}");
                }
                #endregion

                var existingCategory = dbContext.Categories.FirstOrDefault();

                var newCategory = new Category()
                {
                    CategoryName = "NewCategoryName"
                };

                var newProductsList = new List<Product>();
                var product1 = new Product()
                {
                    ProductName = "newProduct1",
                    Category = newCategory,
                    Discontinued = false
                };
                var product2 = new Product()
                {
                    ProductName = "newProduct2",
                    Category = existingCategory,
                    Discontinued = false
                };

                newProductsList.Add(product1);
                newProductsList.Add(product2);

                dbContext.Products.AddRange(newProductsList);
                dbContext.SaveChanges();

                #region After
                Console.WriteLine($"{Environment.NewLine}Список категорий после добавления продукта с новой категорией:");
                currentCategories = dbContext.Categories;
                foreach (var category in currentCategories)
                {
                    Console.WriteLine($"{category.CategoryName}");
                }

                Console.WriteLine($"{Environment.NewLine}Список последних 10 продуктов после добавления:");
                products = dbContext.Products.OrderByDescending(p => p.ProductId).Take(10);
                foreach (var product in products)
                {
                    Console.WriteLine($"{product.ProductName}");
                }
                #endregion

                dbContext.Products.RemoveRange(newProductsList);
                dbContext.Categories.Remove(newCategory);
                dbContext.SaveChanges();
            }
        }

        [Test]
        public void ChangeProductWithAnAnalog()
        {
            #region Before
            using (var dbContext = new NorthwindContext())
            {
                var orders = dbContext.Orders
                    .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                    .ThenInclude(p => p.Category)
                    .ToList();

                var order = orders.Where(o => o.State == OrderState.InWork).FirstOrDefault();

                Console.WriteLine("До:");
                foreach (var od in order.OrderDetails)
                {
                    Console.WriteLine($"OrderID: {od.OrderId} ProductName: {od.Product.ProductName} CategoryName: {od.Product.Category.CategoryName}");
                }
            }
            #endregion

            using (var dbContext = new NorthwindContext())
            {
                var orders = dbContext.Orders
                    .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                    .ThenInclude(p => p.Category)
                    .ToList();

                // В бд нет не отправленных заказов, поэтому использую для примера состояние заказа "OrderState.InWork"
                var order = orders.Where(o => o.State == OrderState.New).FirstOrDefault();
                var newOrderDetails = new List<OrderDetail>();

                foreach (var od in order.OrderDetails)
                {
                    var currentProduct = od.Product;
                    var currentCategory = od.Product.Category;

                    var newProduct = dbContext.Products
                        .Include(p => p.Category)
                        .Where(p => p.Category != null &&
                               p.Category.CategoryName == currentCategory.CategoryName &&
                               p.ProductName != currentProduct.ProductName)
                        .FirstOrDefault();

                    if (newProduct != null)
                    {
                        od.Product = null;
                        dbContext.SaveChanges();
                        od.Product = newProduct;
                        newOrderDetails.Add(od);
                    }
                    else
                    {
                        throw new InvalidOperationException(message: $"В категории {currentCategory.CategoryName} только один продукт");
                    }
                }
                order.OrderDetails = newOrderDetails;
                dbContext.SaveChanges();
            }

            #region After
            using (var dbContext = new NorthwindContext())
            {
                var orders = dbContext.Orders
                    .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                    .ThenInclude(p => p.Category)
                    .ToList();

                var order = orders.Where(o => o.State == OrderState.InWork).FirstOrDefault();

                Console.WriteLine("После:");
                foreach (var od in order.OrderDetails)
                {
                    Console.WriteLine($"OrderID: {od.OrderId} ProductName: {od.Product.ProductName} CategoryName: {od.Product.Category.CategoryName}");
                }
            }
            #endregion
        }
    }
}