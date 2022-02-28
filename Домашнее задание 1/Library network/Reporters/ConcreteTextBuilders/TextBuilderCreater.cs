using Library_network.Models;
using Library_network.Reporters.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_network.Reporters.ConcreteTextBuilders
{
    static class TextBuilderCreater
    {
        public static IReportTextBuilder CreateTextBuilder(Type type)
        {
            return type.Name switch
            {
                "Book" => new BookTextBuilder(),
                "Newspaper" => new NewspaperTextBuilder(),
                "Patent" => new PatentTextBuilder(),
                _ => throw new ArgumentException("Не удалось создать объект для переданного типа", type.GetType().FullName),
            };
        }
    }
}
