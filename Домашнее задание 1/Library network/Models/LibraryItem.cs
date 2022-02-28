using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Library_network.Models
{
    [XmlInclude(typeof(Book))]
    [XmlInclude(typeof(Patent))]
    [XmlInclude(typeof(Newspaper))]
    public abstract class LibraryItem
    {
        public LibraryItem() { }

        public LibraryItem(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public int PublishYear { get; set; }
        public decimal Price { get; set; }
        public int PageCount { get; set; }

        public override bool Equals(object obj)
        {
            if(obj == null || obj is not LibraryItem)
            {
                return false;
            }

            var item = obj as LibraryItem;
            return Id == item.Id || Name == item.Name;
        }
    }
}
