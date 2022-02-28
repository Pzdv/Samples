using Library_network.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Library_network.Models
{
    public class Book : PrintedProduct
    {
        public Book() { }
        public Book(int id, string name) : base(id, name)
        {

        }
        [XmlArrayItem("Autor")]
        [Required(ErrorMessage = "Не установлено значение Autors")]
        public List<string> Autors { get; set; }

        public string ISBN { get; set; }
    }
}
