using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//This is the Refrence Data Type 'Book'

namespace CatalogueOfBooks
{
     public class Book
     {
          // Properties of a Book
          public string ISBN { get; set; }                                 //13 digit ISBN number
          public string Title { get; set; }
          public DateTime DateOfPublication { get; set; }
          public string Publisher { get; set; }
          public List<string> Authors { get; set; }                  //Author(s) of the book
          public string Genre { get; set; }                                // 1 Main Genre of the book
          public string Language { get; set; }
          public string Description { get; set; }
          public decimal Price { get; set; }

          //Borrowing Properties
          public bool IsBorrowed { get; set; }                        //bool to store if the book is borrowed
          public string BorrowedBy { get; set; }                    //Person who Borrowed the book
          public DateTime BorrowDate { get; set; }               //Date when the book was borrowed
          public DateTime DueDate { get; set; }                  //Due date for returning the book

          //Reservation Properties
          public bool IsReserved { get; set; }                        //bool to store if the book is reserved
          public Queue<string> Reservations { get; set; } = new Queue<string>(); //Queue of people who reserved the book

          //History of All People who had borrowed the books
          public List<BookingRecord> BorrowingHistory { get; set; } = new List<BookingRecord>();

          //For Testing Purpose, to check outputs based on dates other than today. (Defaults to Now)
          public DateTime AccessDate { get; set; } = DateTime.Now;

          // Constructor
          public Book(string isbn,
                 string title,
                 DateTime dateOfPublication,
                 string publisher,
                 List<string> authors,
                 string genre,
                 string language,
                 string description,
                 decimal price,
                  DateTime? accessDate = null)
          {
               ISBN = isbn;
               Title = title;
               DateOfPublication = dateOfPublication;
               Publisher = publisher;
               Authors = authors ; 
               Genre = genre;
               Language = language;
               Description = description;
               Price = price;

               // Initialize borrowing/reservation status (defaults)
               IsBorrowed = false;
               BorrowedBy = null;
               BorrowDate = DateTime.MinValue;
               IsReserved = false;

               // Initialize DateToday
               AccessDate = accessDate ?? DateTime.Now;
          }

          //Default Constructor for Json Loading from file
          public Book()
          {
               BorrowingHistory = new List<BookingRecord>(); 
          }

          //Borrow the Book
          public bool Borrow(string user)
          {
               // Book is already borrowed
               if (IsBorrowed)
                    return false;

               // Successfully borrowed
               IsBorrowed = true;
               BorrowedBy = user;
               BorrowDate = DateTime.Now;
               DueDate = BorrowDate.AddDays(10);  // 10-day borrow period
               return true;  
          }

          //Returning the Book
          public bool Return()
          {
               // Book is not currently borrowed
               if (!IsBorrowed)
                    return false;

               // Record the borrowing history before resetting
               AddBorrowingRecord();

               // End current borrowing
               IsBorrowed = false;
               BorrowedBy = null;
               BorrowDate = DateTime.MinValue.Date;
               DueDate = DateTime.MinValue.Date;

               // Check for pending reservations
               if (Reservations.Count > 0)
               {
                    // Assign the book to the next user
                    IsBorrowed = true;
                    BorrowedBy = Reservations.Dequeue();
                    BorrowDate = DateTime.Today.Date;            
                    DueDate = BorrowDate.AddDays(10);          

                    // IsReserved = true if more reservations remain
                    IsReserved = Reservations.Count > 0;
               }
               else
                    IsReserved = false;
               
               // Successfully returned (and maybe re-borrowed)
               return true;
          }


          //Making a Reservation
          public bool Reserve(string userName)
          {
               if (IsBorrowed)
               {
                    // Add reservation to the queue (Sorted Dictionary)
                    Reservations.Enqueue(userName);
                    IsReserved = true;
                    return true;
               }
               return false; // Book is not borrowed, so reservation cannot be made
          }
          
          //Calculating the Book's time from today.
          public decimal CalculateFine() { 
               // Checking if book is Borrowed, & actually Overdue
               if (IsBorrowed && (AccessDate > DueDate))
               {
                    TimeSpan overdueDays = AccessDate - DueDate;
                    return (decimal)overdueDays.TotalDays * 0.50m; // 0.50 per day
               }

               // No fine if not overdue or not borrowed
               return 0.0m;
           }

          //Add a Record of Book Borowing (when it is returned)
          private void AddBorrowingRecord()
          {
               BorrowingHistory.Add(new BookingRecord
               {
                    Borrower = BorrowedBy,
                    BorrowDate = BorrowDate.Date,
                    ReturnDate = DateTime.Now.Date,
                    FinePaid = CalculateFine()
               });
          }
     }
}
