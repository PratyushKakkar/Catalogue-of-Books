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
               Random rand = new Random();
               decimal randomDecimal = 1.0m + ((decimal)rand.NextDouble() * 0.01m);
               Console.WriteLine(randomDecimal + " " +( 1.0m + ((decimal)rand.NextDouble() * 0.01m)));

               BookInventory bi = new BookInventory(@"C:\Users\Anku\Desktop\Class Notes\Year 2\Algorithm & Data Structure\Catalogue-of-Books\CatalogueOfBooks\Books.json");
               Book b1 = new Book(
                   isbn: "9783161484100",
                   title: "1. The Great Novel",
                   dateOfPublication: new DateTime(2023, 1, 15),
                   publisher: "Literary Press",
                   authors: new List<string> { "John Doe", "Jane Smith" },
                   genre: "Fiction",
                   language: "English",
                   description: "A captivating story about...",
                   price: 24.99m
               );
               bi.Add(b1);

               bool result = bi.Borrow("9783161484100", "Borrow1", DateTime.Now);
               Console.WriteLine("borrow" + result);

               result = bi.Reserve("9783161484100", DateTime.Now.AddDays(15), "Reserve2");
               Console.WriteLine("reserve1" + result);

               bi.EditReservationName("9783161484100", DateTime.Now.AddDays(15), "Reserve2 new");

               result = bi.Reserve("9783161484100", DateTime.Now.AddDays(20), "Reserve3");

               bi.EditReservationDate("9783161484100", DateTime.Now.AddDays(20), DateTime.Now.AddDays(40));

               bi.Reserve("9783161484100", DateTime.Now.AddDays(30), "Reserve4");

               bi.Return("9783161484100");

               Book b2 = new Book
               (
                    isbn: "9781234567897",
                    title: "2. The Mystery of the Sea",
                    dateOfPublication: new DateTime(2021, 6, 10),
                    publisher: "Oceanic Publications",
                    authors: new List<string> { "Emily White", "Michael Brown" },
                    genre: "Mystery",
                    language: "English",
                    description: "A thrilling tale of adventure and suspense on the high seas.",
                    price: 19.99m
               );

               bi.Add(b2);
               bi.Borrow(b2.ISBN, "user5", DateTime.Now.AddDays(10));

               Book b3 = new Book("9780451524935",
                    "3. The Adventure Begins",
                    new DateTime(2022, 3, 20),
                    "Grand Publishing",
                    new List<string> { "Alice Johnson", "David Lee" },
                    "Adventure",
                    "English",
                    "An epic journey through uncharted lands and thrilling encounters.",
                    29.99m);
               bi.Add(b3);

               SortedList<decimal, Book> books = bi.SearchByPublisher("publis");

               foreach (KeyValuePair<decimal, Book> entry in books.Reverse())
               {
                    Book book = entry.Value;
                    DisplayBook(book);
               }

               Console.ReadLine();
          }
          public static void DisplayBook(Book book)
          {
               Console.WriteLine("Book Details:");
               Console.WriteLine("--------------------------------------------------");
               Console.WriteLine($"ISBN: {book.ISBN}");
               Console.WriteLine($"Title: {book.Title}");
               Console.WriteLine($"Publication Date: {book.DateOfPublication.ToString("d MMMM yyyy")}");
               Console.WriteLine($"Publisher: {book.Publisher}");

               Console.WriteLine("Authors:");
               foreach (var author in book.Authors)
               {
                    Console.WriteLine($"  - {author}");
               }

               Console.WriteLine($"Genre: {book.Genre}");
               Console.WriteLine($"Language: {book.Language}");
               Console.WriteLine($"Description: {book.Description}");
               Console.WriteLine($"Price: {book.Price:C}");

               Console.WriteLine($"Is Borrowed: {book.IsBorrowed}");
               if (book.IsBorrowed)
               {
                    Console.WriteLine($"  Borrowed By: {book.BorrowedBy}");
                    Console.WriteLine($"  Borrow Date: {book.BorrowDate.ToString("d MMMM yyyy")}");
                    Console.WriteLine($"  Due Date: {book.DueDate.ToString("d MMMM yyyy")}");
               }

               Console.WriteLine($"Is Reserved: {book.IsReserved}");
               if (book.IsReserved)
               {
                    Console.WriteLine("Reservations:");
                    foreach (var reservation in book.Reservations)
                    {
                         Console.WriteLine($"  - {reservation.Value} on {reservation.Key.ToString("d MMMM yyyy")}");
                    }
               }

               Console.WriteLine("Borrowing History:");
               foreach (var record in book.BorrowingHistory)
               {
                    Console.WriteLine($"  Borrower: {record.Borrower}, Borrow Date: {record.BorrowDate.ToString("d MMMM yyyy")}, Return Date: {record.ReturnDate.ToString("d MMMM yyyy")}, Fine Paid: {record.FinePaid:C}");
               }

               Console.WriteLine("--------------------------------------------------");
          }

          }
     }
