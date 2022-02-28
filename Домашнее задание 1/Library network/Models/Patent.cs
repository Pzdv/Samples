using Library_network.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Library_network.Models
{
    public class Patent : LibraryItem
    {
        public Patent() { }
        public Patent(int id, string name) : base(id, name)
        {

        }

        [XmlArrayItem("Inventor")]
        public List<string> Inventors { get; set; }
        public string Country { get; set; }
        public string RegistrationNumber { get; set; }
        [XmlElement(DataType = "date")]
        public DateTime FilingDate { get; set; }
        

    }
}
