using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Library_network.Models
{
    public class Newspaper : PrintedProduct
    {
        public Newspaper() { }
        public Newspaper(int id, string name) : base(id, name)
        {

        }

        public int Number { get; set; }
        [XmlElement(DataType = "date")]
        public DateTime? Date { get; set; }
        public string ISSN { get; set; }
    }
}
