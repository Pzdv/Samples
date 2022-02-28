using Northwind.Enums;
using System;

namespace Northwind.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public string CustomerID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime? OrderDate { get; private set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; private set; }
        public int ShipVia { get; set; }
        public decimal? Freight { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipRegion { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }
        public OrderState State { get; private set; }


        internal void SetOrderDate(DateTime? orderDate)
        {
            if (orderDate != null)
            {
                OrderDate = orderDate;
                State = OrderState.InWork;
            }
        }

        internal void SetShippedDate(DateTime? shippedDate)
        {
            if (shippedDate != null)
            {
                ShippedDate = shippedDate;
                State = OrderState.Completed;
            }
        }
    }
}
