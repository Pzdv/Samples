using Library_network.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_network.Models
{
    public abstract class PrintedProduct : LibraryItem
    {
        public PrintedProduct() { }
        public PrintedProduct(int id, string name) : base(id, name)
        {

        }

        public string CreationCity { get; set; }
        public string PublishingHouse { get; set; }
        public int InstanceCount { get; set; }
    }
}
