using System;
using System.Collections.Generic;
using System.IO;
using CatalogueOfBooks;

class Program
{
     static void Main(string[] args)
     {
          //Json File stored in the Base Directory (Catalogue-of-Books)
          string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Books.json");
          BookInventory inventory = new BookInventory(filePath);
          inventory.LoadFromJson();

          //Book Menu
          while (true)
          {
               Console.WriteLine("\n--- Book Inventory Menu ---");
               Console.WriteLine("1. View All Books");
               Console.WriteLine("2. Add Book");
               Console.WriteLine("3. Remove Book");
               Console.WriteLine("4. Edit Book");
               Console.WriteLine("5. Borrow Book");
               Console.WriteLine("6. Return Book");
               Console.WriteLine("7. Reserve Book");
               Console.WriteLine("8. Calculate Fine");
               Console.WriteLine("9. Search Books");
               Console.WriteLine("10. View Borrowed Books");
               Console.WriteLine("11. View Reserved Books");
               Console.WriteLine("12. Exit");
               Console.Write("Enter your choice (1-12): ");
               string choice = Console.ReadLine();

               switch (choice)
               {
                    case "1":
                         foreach (Book book in inventory.GetAllBooks())
                              PrintBookDetails(book);
                         break;

                    case "2":
                         Book newBook = ReadBookFromInput();
                         bool added = inventory.Add(newBook);
                         Console.WriteLine(added ? "Book added successfully." : "Book with this ISBN already exists!");
                         break;

                    case "3":
                         Console.Write("Enter ISBN to remove: ");
                         string removeIsbn = Console.ReadLine();
                         bool removed = inventory.Remove(removeIsbn);
                         Console.WriteLine(removed ? "Book removed." : "Book not found.");
                         break;

                    case "4":
                         Console.WriteLine("Enter updated book details:");
                         Book updatedBook = ReadBookFromInput();
                         bool edited = inventory.EditBook(updatedBook);
                         Console.WriteLine(edited ? "Book updated." : "Book not found.");
                         break;

                    case "5":
                         Console.Write("Enter ISBN to borrow: ");
                         string borrowIsbn = Console.ReadLine();
                         Console.Write("Enter your name: ");
                         string borrower = Console.ReadLine();
                         bool borrowed = inventory.Borrow(borrowIsbn, borrower);
                         Console.WriteLine(borrowed ? "Book borrowed." : "Could not borrow!");
                         break;

                    case "6":
                         Console.Write("Enter ISBN to return: ");
                         string returnIsbn = Console.ReadLine();
                         bool returned = inventory.Return(returnIsbn);
                         Console.WriteLine(returned ? "Book returned." : "Book not found or not borrowed!");
                         break;

                    case "7":
                         Console.Write("Enter ISBN to reserve: ");
                         string reserveIsbn = Console.ReadLine();
                         Console.Write("Enter your name: ");
                         string reserver = Console.ReadLine();
                         bool reserved = inventory.Reserve(reserveIsbn, reserver);
                         Console.WriteLine(reserved ? "Book reserved." : "Book not found!");
                         break;

                    case "8":
                         Console.Write("Enter ISBN to calculate fine: ");
                         string fineIsbn = Console.ReadLine();
                         decimal fine = inventory.CalculateFineToday(fineIsbn);
                         Console.WriteLine("Fine: €" + fine);
                         break;

                    case "9":
                         RunSearchMenu(inventory);
                         break;

                    case "10":
                         foreach (Book book in inventory.GetAllBorrowedBooks())
                              PrintBookDetails(book);
                         break;

                    case "11":
                         foreach (Book book in inventory.GetAllReservedBooks())
                              PrintBookDetails(book);
                         break;

                    case "12":
                         Console.WriteLine("Thankyou!");
                         return;

                    default:
                         Console.WriteLine("Invalid choice!");
                         break;
               }
          }
     }

     // Method to print all details of a book
     static void PrintBookDetails(Book book)
     {
          Console.WriteLine("===== Book Details =====");
          Console.WriteLine($"Title: {book.Title}");
          Console.WriteLine($"ISBN: {book.ISBN}");
          Console.WriteLine($"Publisher: {book.Publisher}");
          Console.WriteLine($"Genre: {book.Genre}");
          Console.WriteLine($"Language: {book.Language}");
          Console.WriteLine($"Description: {book.Description}");
          Console.WriteLine($"Price: {book.Price:C}");
          Console.WriteLine($"Date of Publication: {book.DateOfPublication.ToShortDateString()}");

          // Authors
          Console.WriteLine("Authors: ");
          foreach (var author in book.Authors)
          {
               Console.WriteLine($"  - {author}");
          }

          // Borrowing Information
          Console.WriteLine($"Is Borrowed: {book.IsBorrowed}");
          if (book.IsBorrowed)
          {
               Console.WriteLine($"Borrowed By: {book.BorrowedBy}");
               Console.WriteLine($"Borrow Date: {book.BorrowDate.ToShortDateString()}");
               Console.WriteLine($"Due Date: {book.DueDate.ToShortDateString()}");
          }

          // Reservation Information
          Console.WriteLine($"Is Reserved: {book.IsReserved}");
          if (book.IsReserved)
          {
               Console.WriteLine($"Reservations: ");
               foreach (var reserver in book.Reservations)
               {
                    Console.WriteLine($"  - {reserver}");
               }
          }

          // Borrowing History
          Console.WriteLine("Borrowing History: ");
          foreach (var record in book.BorrowingHistory)
          {
               Console.WriteLine($"  - Borrower: {record.Borrower}, Borrow Date: {record.BorrowDate.ToShortDateString()}, Return Date: {record.ReturnDate.ToShortDateString()}, Fine Paid: {record.FinePaid:C}");
          }

          Console.WriteLine("========================");
     }

     //Inputting Details of a Book
     static Book ReadBookFromInput()
     {
          Console.Write("ISBN: ");
          string isbn = Console.ReadLine();
          Console.Write("Title: ");
          string title = Console.ReadLine();
          Console.Write("Publisher: ");
          string publisher = Console.ReadLine();
          Console.Write("Authors (comma separated): ");
          List<string> authors = new List<string>(Console.ReadLine().Split(','));
          Console.Write("Genre: ");
          string genre = Console.ReadLine();
          Console.Write("Language: ");
          string language = Console.ReadLine();
          Console.Write("Description: ");
          string description = Console.ReadLine();
          Console.Write("Price: ");
          decimal price = decimal.Parse(Console.ReadLine());
          Console.Write("Year of Publication: ");
          int year = int.Parse(Console.ReadLine());

          return new Book(
              isbn,
              title,
              new DateTime(year, 1, 1),
              publisher,
              authors,
              genre,
              language,
              description,
              price
          );
     }

     //Option 9 of Book Menu
     static void RunSearchMenu(BookInventory inventory)
     {
          Console.WriteLine("\n--- Search Menu ---");
          Console.WriteLine("(Returns All Books that Match)");
          Console.WriteLine("1. By ISBN");
          Console.WriteLine("2. By Title");
          Console.WriteLine("3. By Year");
          Console.WriteLine("4. By Publisher");
          Console.WriteLine("5. By Author");
          Console.WriteLine("6. By Genre");
          Console.WriteLine("7. By Language");
          Console.WriteLine("8. In Description");
          Console.WriteLine("9. By Price Range");
          Console.Write("Enter your choice (1-9): ");
          string option = Console.ReadLine();

          switch (option)
          {
               case "1":
                    Console.Write("Enter ISBN: ");
                    var isbnResults = inventory.SearchByISBN(Console.ReadLine());
                    foreach (var book in isbnResults.Values) 
                         PrintBookDetails(book);
                    break;

               case "2":
                    Console.Write("Enter title: ");
                    var titleResults = inventory.SearchByTitle(Console.ReadLine());
                    foreach (var book in titleResults.Values) 
                         PrintBookDetails(book);
                    break;

               case "3":
                    Console.Write("Enter year: ");
                    int year = int.Parse(Console.ReadLine());
                    var yearResults = inventory.SearchByPublicationYear(year);
                    foreach (var book in yearResults) 
                         PrintBookDetails(book);
                    break;

               case "4":
                    Console.Write("Enter publisher: ");
                    var pubResults = inventory.SearchByPublisher(Console.ReadLine());
                    foreach (var book in pubResults.Values) 
                         PrintBookDetails(book);
                    break;

               case "5":
                    Console.Write("Enter author: ");
                    var authorResults = inventory.SearchByAuthor(Console.ReadLine());
                    foreach (var book in authorResults.Values) 
                         PrintBookDetails(book);
                    break;

               case "6":
                    Console.Write("Enter genre: ");
                    var genreResults = inventory.SearchByGenre(Console.ReadLine());
                    foreach (var book in genreResults.Values) 
                         PrintBookDetails(book);
                    break;

               case "7":
                    Console.Write("Enter language: ");
                    var langResults = inventory.SearchByLanguage(Console.ReadLine());
                    foreach (var book in langResults.Values) 
                         PrintBookDetails(book);
                    break;

               case "8":
                    Console.Write("Enter text: ");
                    var descResults = inventory.SearchInDescription(Console.ReadLine());
                    foreach (var book in descResults.Values) 
                         PrintBookDetails(book);
                    break;

               case "9":
                    Console.Write("Min price: ");
                    decimal min = decimal.Parse(Console.ReadLine());
                    Console.Write("Max price: ");
                    decimal max = decimal.Parse(Console.ReadLine());
                    var priceResults = inventory.SearchByPriceRange(min, max);
                    foreach (var book in priceResults) 
                         PrintBookDetails(book);
                    break;

               default:
                    Console.WriteLine("Invalid search option.");
                    break;
          }
     }
}