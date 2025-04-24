using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSA_Assign_2
{
    class Program
    {
        static void Main(string[] args)
        {
            CatalogueOfBooks catalogue = new CatalogueOfBooks();
            List<Book> books = catalogue.LoadBooks();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Library Management System (Librarian Interface) ===");
                Console.WriteLine("1. Add New Book");
                Console.WriteLine("2. View All Books");
                Console.WriteLine("3. Remove a Book");
                Console.WriteLine("4. Search Book");
                Console.WriteLine("5. Edit Book Details");
                Console.WriteLine("6. Borrow a Book");
                Console.WriteLine("7. Return a Book");
                Console.WriteLine("8. View Sorted Book List (by Year → Title)");
                Console.WriteLine("9. View Borrowed Books");
                Console.WriteLine("10. View Reserved Books");
                Console.WriteLine("11. Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        LibraryService.AddBook(books);
                        break;
                    case "2":
                        LibraryService.ViewBooks(books);
                        break;
                    case "3":
                        LibraryService.RemoveBook(books);
                        break;
                    case "4":
                        LibraryService.SearchBook(books);
                        break;
                    case "5":
                        LibraryService.EditBookDetails(books);
                        break;
                    case "6":
                        LibraryService.BorrowBook(books);
                        break;
                    case "7":
                        LibraryService.ReturnBook(books);
                        break;
                    case "8":
                        LibraryService.ViewSortedBooks(books);
                        break;
                    case "9":
                        LibraryService.ViewBorrowedBooks(books);
                        break;
                    case "10":
                        LibraryService.ViewReservedBooks(books);
                        break;
                    case "11":
                        catalogue.SaveBooks(books);
                        return;
                    default:
                        Console.WriteLine("Invalid option. Press Enter to continue.");
                        Console.ReadLine();
                        break;
                }

                catalogue.SaveBooks(books);
            }
        }
    }
}

