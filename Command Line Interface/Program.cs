using CatalogueOfBooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Command_Line_Interface
{
     internal class Program
     {
          static void Main(string[] args)
          {
               BookInventory bi = new BookInventory(@"C:\Users\Anku\Desktop\Class Notes\Year 2\Algorithm & Data Structure\Catalogue-of-Books\CatalogueOfBooks\Books.json");
               Book b1 = new Book("978-3-16-148410-0", "The Great Gatsby", new DateTime(1925, 4, 10), "Scribner", new List<string> { "F. Scott Fitzgerald" }, "Fiction", "English", "A novel set in the Roaring Twenties.", 10.99m);
               bi.Add(b1);
               Console.ReadLine();
          }
     }
}
