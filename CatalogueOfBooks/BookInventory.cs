using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
//Package to compare Similarity between strings
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

          public bool EditBook(Book updatedBook)
          {
               foreach (Book existingBook in books)
               {
                    if (updatedBook.ISBN == existingBook.ISBN)
                    {
                         Remove(existingBook.ISBN);
                         Add(updatedBook);
                         SaveToFile();
                         return true;
                    }   
               }
               return false;       //Book Not Found
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

          // Search by ISBN (Very pricise match)
          public SortedList<decimal, Book> SearchByISBN(string isbn)
          {
               SortedList<decimal, Book> sortedBooks = new SortedList<decimal, Book>();

               // Iterate through the list of books
               foreach (Book book in books)
               {
                    // Generate a very small random number between 1.00 and 1.01
                    decimal randomDecimal = 1.0m + ((decimal)rand.NextDouble() * 0.01m);

                    //Multiplies with score (level of matching) to avoid duplicate keys.
                    decimal matchScore = Fuzz.WeightedRatio(isbn, book.ISBN) * randomDecimal;

                    // If the match score is very similar, add to List
                    if (matchScore > 90)
                    {
                         sortedBooks.Add(matchScore, book);
                    }
               }
               return sortedBooks;
          }


          // Search by Title (partial match)
          public SortedList<decimal, Book> SearchByTitle(string title)
          {
               SortedList<decimal, Book> sortedBooks = new SortedList<decimal, Book>();

               foreach (Book book in books)
               {
                    // Generate a very small random number between 1.00 and 1.01
                    decimal randomDecimal = 1.0m + ((decimal)rand.NextDouble() * 0.01m);

                    //Multiplies with score (level of matching) to avoid duplicate keys.
                    decimal matchScore = Fuzz.WeightedRatio(title, book.Title) * randomDecimal;

                    //Add to List if match is above average
                    if (matchScore > 70)
                         sortedBooks.Add(matchScore, book);
                    
               }
               return sortedBooks;
          }


          // Search by Publication Year (Exact)
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

                    //Multiplies with score (level of matching) to avoid duplicate keys.
                    decimal matchScore = Fuzz.WeightedRatio(publisher, book.Publisher) * randomDecimal;

                    //Adds if the match is above average
                    if(matchScore > 70)
                         sortedBooks.Add(matchScore, book);
               }

               return sortedBooks;
          }

          // Search by Author (partial match, case-insensitive)
          public SortedList<decimal, Book> SearchByAuthor(string author)
          {
               SortedList<decimal, Book> sortedBooks = new SortedList<decimal, Book>();

               foreach (Book book in books)
               {
                    foreach (string bookAuthor in book.Authors)
                    {
                         decimal randomDecimal = 1.0m + ((decimal)rand.NextDouble() * 0.01m);

                         decimal matchScore = Fuzz.WeightedRatio(author, bookAuthor) * randomDecimal;

                         if (matchScore > 70)
                         {
                              sortedBooks.Add(matchScore, book);
                              break;
                         }
                    }
               }
               return sortedBooks;
          }

          // Search by Genre (exact match, case-insensitive)
          public SortedList<decimal, Book> SearchByGenre(string genre)
          {
               SortedList<decimal, Book> sortedBooks = new SortedList<decimal, Book>();

               foreach (Book book in books)
               {
                    decimal randomDecimal = 1.0m + ((decimal)rand.NextDouble() * 0.01m);

                    decimal matchScore = Fuzz.WeightedRatio(genre, book.Genre) * randomDecimal;

                    if (matchScore > 80)
                    {
                         sortedBooks.Add(matchScore, book);
                    }
               }
               return sortedBooks;
          }

          // Search by Language (exact match, case-insensitive)
          public SortedList<decimal, Book> SearchByLanguage(string language)
          {
               SortedList<decimal, Book> sortedBooks = new SortedList<decimal, Book>();

               foreach (Book book in books)
               {
                    decimal randomDecimal = 1.0m + ((decimal)rand.NextDouble() * 0.01m);

                    decimal matchScore = Fuzz.WeightedRatio(language, book.Language) * randomDecimal;

                    if (matchScore > 90)
                    {
                         sortedBooks.Add(matchScore, book);
                    }
               }
               return sortedBooks;
          }

          // Search in Description (partial match, case-insensitive)
          public SortedList<decimal, Book> SearchInDescription(string text)
          {
               SortedList<decimal, Book> sortedBooks = new SortedList<decimal, Book>();

               foreach (Book book in books)
               {
                    if (book.Description != null)
                    {
                         decimal randomDecimal = 1.0m + ((decimal)rand.NextDouble() * 0.01m);

                         decimal matchScore = Fuzz.WeightedRatio(text, book.Description) * randomDecimal;

                         if (matchScore > 70)
                         {
                              sortedBooks.Add(matchScore, book);
                         }
                    }
               }
               return sortedBooks;
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
                              book.Reservations = new SortedList<DateTime, string>();

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
