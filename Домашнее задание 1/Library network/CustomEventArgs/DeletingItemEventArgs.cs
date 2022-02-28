using Library_network.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_network.CustomEventArgs
{
    public class DeletingItemEventArgs : EventArgs 
    {
        public LibraryItem DeletingItem { get; }
        public bool Abort { get; set; }

        public DeletingItemEventArgs(LibraryItem deletingItem)
        {
            DeletingItem = deletingItem;
            Abort = false;
        }
    }
}
