using CatalogueOfBooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Graphical_User_Interface
{
     //Class to Share the same Instance of BookInventory
     public static class SharedInventory
     {
          public static BookInventory inventory;

          static SharedInventory()
          {
               string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Books.json");
               inventory = new BookInventory(filePath);
          }

     }
}
