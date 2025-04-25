using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace CatalogueOfBooks
{
     public class BookInventory 
     {
          private List<Book> books;
          private readonly string filePath;

          public BookInventory(string file)
          {
              LoadFromFile(file);
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

          public bool Reserve(string ISBN, Reservation reservation)
          {
               foreach (Book existingBook in books)
               {
                    if (existingBook.ISBN == ISBN)
                    {
                         existingBook.Reserve(reservation);
                         return true; // Book reserved successfully
                    }
               }
               return false; // Book not found
          }
          
          public bool Borrow(string ISBN, string user, DateTime borrowDate)
          {
               foreach (Book existingBook in books)
               {
                    if (existingBook.ISBN == ISBN)
                         return existingBook.Borrow(user, borrowDate);
                    
               }
               return false; // Book not found
          }

          public bool Return(string ISBN)
          {
               foreach (Book existingBook in books)
               {
                    if (existingBook.ISBN == ISBN)
                         return existingBook.Return();

               }
               return false; // Book not found
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
          public List<Book> SearchByPublisher(string publisher)
          {
               List<Book> results = new List<Book>();
               foreach (Book book in books)
               {
                    if (book.Publisher.IndexOf(publisher, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                         results.Add(book);
                    }
               }
               return results;
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
                    if(string.IsNullOrEmpty(json))
                         return;
                    books = JsonSerializer.Deserialize<List<Book>>(json);
               }
          }

          // Save books to file
          private void SaveToFile()
          {
               JsonSerializerOptions options = new JsonSerializerOptions();
               options.WriteIndented = true;
               string json = JsonSerializer.Serialize(books, options);
               File.WriteAllText(filePath, json);
          }
     }
}
