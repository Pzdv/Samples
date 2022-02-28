using Library_network.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_network.CustomEventArgs
{
    public class AddedItemEventArgs : EventArgs
    {
        public Type ItemType { get; }

        public AddedItemEventArgs(Type itemType) : base()
        {
            ItemType = itemType;
        }
    }
}
