using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using FuzzySharp;

namespace CatalogueOfBooks
{
     public class BookInventory 
     {
          public List<Book> books = new List<Book>();
          private readonly string filePath;

          //Object to generate Random numbers
          Random rand = new Random();

          public BookInventory(string file)
          {
              // TEMPORARYILY LoadFromFile(file);
               filePath = file;
          }

          public bool Add(Book book)
          { 
               // Check if Book already exists
               foreach (Book existingBook in books)
               {
                    if (existingBook.ISBN == book.ISBN)
                         return false; // ISBN already exists
               }

               books.Add(book);
               SaveToFile();

               // Book added successfully
               return true; 
          }

          public bool Remove(string ISBN)
          {
               foreach (Book existingBook in books) 
               { 
                    if(existingBook.ISBN == ISBN)
                    {
                         books.Remove(existingBook);
                         SaveToFile();
                         return true; // Book removed successfully
                    }
               }
               return true;
          }


          
          public bool Borrow(string ISBN, string user, DateTime borrowDate)
          {
               bool borrowed = false;
               foreach (Book existingBook in books)
               {
                    if (existingBook.ISBN == ISBN)
                    {
                         borrowed = existingBook.Borrow(user, borrowDate.Date);
                         SaveToFile();
                         return borrowed;
                    }
               }
               return false; // Book not found
          }

          public bool Return(string ISBN)
          {
               foreach (Book existingBook in books)
               {
                    if (existingBook.ISBN == ISBN)
                    {
                         bool result = existingBook.Return();
                         SaveToFile();
                         return result;
                    }

               }
               return false; // Book not found
          }

          public bool Reserve(string ISBN, DateTime reservationDate, string userName)
          {
               bool reserved = false;
               foreach (Book existingBook in books)
               {
                    if (existingBook.ISBN == ISBN)
                    {
                         reserved = existingBook.Reserve(reservationDate.Date, userName);
                         SaveToFile();
                         return reserved; // Book reserved successfully
                    }
               }
               return false; // Book not found
          }

          // Updates the name of the user for a specific reservation date for a specific book
          public bool EditReservationName(string ISBN, DateTime reservationDate, string newName)
          {
               foreach (Book existingBook in books)
               {
                    if (existingBook.ISBN == ISBN)
                    {
                         bool updated = existingBook.EditReservationName(reservationDate.Date, newName);
                         if (updated)
                         {
                              SaveToFile();
                         }
                         return updated;
                    }
               }
               return false; // Book not found
          }

          // Updates the reservation date for a specific user's reservation for a specific book
          public bool EditReservationDate(string ISBN, DateTime oldReservationDate, DateTime newReservationDate)
          {
               foreach (Book existingBook in books)
               {
                    if (existingBook.ISBN == ISBN)
                    {
                         bool updated = existingBook.EditReservationDate(oldReservationDate.Date, newReservationDate.Date);
                         if (updated)
                         {
                              SaveToFile();
                         }
                         return updated;
                    }
               }
               return false; // Book not found
          }

          public decimal CalculateFineToday(string ISBN)
          {
               foreach(Book existingBook in books)
               {
                    if (existingBook.ISBN == ISBN)
                         return existingBook.CalculateFine();
               }
               return 0.0m;
          }

          //Books Sorted by Year and then Title
          public List<Book> GetAllBooks()
          {
               //LINQ Query to Order books by Year & then Title, in Ascending Order.
               List<Book> sortedBooks = (from book in books 
                                                               orderby book.DateOfPublication.Year, book.Title 
                                                               select book).ToList();
               return sortedBooks;
          }

          //Returns List of Borrowed Books
          public List<Book> GetAllBorrowedBooks()
          {
               //LINQ Query to Select All Books that are Borrowed
               List<Book> sortedBooks = (from book in books
                                                             where book.IsBorrowed == true
                                                             select book).ToList();
               return sortedBooks;
          }

          //Returns List of Reserved Books
          public List<Book> GetAllReservedBooks()
          {
               //LINQ Query to Select All Books that are Borrowed
               List<Book> sortedBooks = (from book in books
                                         where book.IsReserved == true
                                         select book).ToList();
               return sortedBooks;
          }

          // Search by ISBN (exact match)
          public Book SearchByISBN(string isbn)
          {
               foreach (Book book in books)
               {
                    if (book.ISBN == isbn)
                    {
                         return book;
                    }
               }
               return null;
          }

          // Search by Title (partial match, case-insensitive)
          public List<Book> SearchByTitle(string title)
          {
               List<Book> bookWithTitle = new List<Book>();

               foreach (Book book in books)
               {
                    if (book.Title.IndexOf(title, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                         bookWithTitle.Add(book);
                    }
               }
               return bookWithTitle;
          }

          // Search by Publication Year
          public List<Book> SearchByPublicationYear(int year)
          {
               List<Book> results = new List<Book>();
               foreach (Book book in books)
               {
                    if (book.DateOfPublication.Year == year)
                    {
                         results.Add(book);
                    }
               }
               return results;
          }

          // Search by Publisher (partial match, case-insensitive)
          public SortedList<decimal, Book> SearchByPublisher(string publisher)
          {
               SortedList<decimal, Book> sortedBooks = new SortedList<decimal, Book>();

               //Ranks Books based on Match found
               foreach (Book book in books)
               {
                    //Generate a very small random num between 1.00 & 1.01
                    decimal randomDecimal = 1.0m + ((decimal)rand.NextDouble() * 0.01m);

                    //Multiplies with score to avoid duplicate keys.
                    decimal matchScore = Fuzz.WeightedRatio(publisher, book.Publisher) * randomDecimal;
                    sortedBooks.Add(matchScore, book);
               }

               return sortedBooks;
          }

          // Search by Author (partial match, case-insensitive)
          public List<Book> SearchByAuthor(string author)
          {
               List<Book> results = new List<Book>();
               foreach (Book book in books)
               {
                    foreach (string bookAuthor in book.Authors)
                    {
                         if (bookAuthor.IndexOf(author, StringComparison.OrdinalIgnoreCase) >= 0)
                         {
                              results.Add(book);
                              break;
                         }
                    }
               }
               return results;
          }

          // Search by Genre (exact match, case-insensitive)
          public List<Book> SearchByGenre(string genre)
          {
               List<Book> results = new List<Book>();
               foreach (Book book in books)
               {
                    if (string.Equals(book.Genre, genre, StringComparison.OrdinalIgnoreCase))
                    {
                         results.Add(book);
                    }
               }
               return results;
          }

          // Search by Language (exact match, case-insensitive)
          public List<Book> SearchByLanguage(string language)
          {
               List<Book> results = new List<Book>();
               foreach (Book book in books)
               {
                    if (string.Equals(book.Language, language, StringComparison.OrdinalIgnoreCase))
                    {
                         results.Add(book);
                    }
               }
               return results;
          }

          // Search in Description (partial match, case-insensitive)
          public List<Book> SearchInDescription(string text)
          {
               List<Book> results = new List<Book>();
               foreach (Book book in books)
               {
                    if (book.Description != null &&
                        book.Description.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                         results.Add(book);
                    }
               }
               return results;
          }

          // Search by Price Range
          public List<Book> SearchByPriceRange(decimal minPrice, decimal maxPrice)
          {
               List<Book> results = new List<Book>();
               foreach (Book book in books)
               {
                    if (book.Price >= minPrice && book.Price <= maxPrice)
                    {
                         results.Add(book);
                    }
               }
               return results;
          }

          // Load books from file, if file exists & is not empty
          private void LoadFromFile(string filePath)
          {
               if (File.Exists(filePath))
               {
                    string json = File.ReadAllText(filePath);
                    if (string.IsNullOrEmpty(json))
                         return;

                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                         IncludeFields = true
                    };

                    books = JsonSerializer.Deserialize<List<Book>>(json, options) ?? new List<Book>();

                    // Fix any null collections after loading
                    foreach (var book in books)
                    {
                         if (book.Reservations == null)
                              book.Reservations = new SortedDictionary<DateTime, string>();

                         if (book.BorrowingHistory == null)
                              book.BorrowingHistory = new List<BookingRecord>();
                    }
               }
          }


          // Save books to file
          private void SaveToFile()
          {
               JsonSerializerOptions options = new JsonSerializerOptions
               {
                    WriteIndented = true,
                    IncludeFields = true
               };
               string json = JsonSerializer.Serialize(this, options);
               File.WriteAllText(filePath, json);
          }
     }
}
