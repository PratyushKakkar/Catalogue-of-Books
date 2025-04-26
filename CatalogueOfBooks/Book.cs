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
          public SortedDictionary<DateTime, string> Reservations = new SortedDictionary<DateTime, string>(); //Reservation Date is the unique key, user name is value 

          //History of All People who had borrowed the books
          public List<BookingRecord> BorrowingHistory = new List<BookingRecord>();

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

          //Borrow the Book
          public bool Borrow(string user, DateTime borrowDate)
          {
               // Book is already borrowed
               if (IsBorrowed)
                    return false;

               // Successfully borrowed
               IsBorrowed = true;
               BorrowedBy = user;
               BorrowDate = borrowDate;
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
               BorrowDate = DateTime.MinValue;
               DueDate = DateTime.MinValue;

               // Check for pending reservations
               if (Reservations.Count > 0)
               {
                    // Get the first reservation
                    var nextReservation = Reservations.First();

                    // Remove it from the dictionary
                    Reservations.Remove(nextReservation.Key);

                    // Assign the book to the next user
                    IsBorrowed = true;
                    BorrowedBy = nextReservation.Value;
                    BorrowDate = nextReservation.Key;
                    DueDate = BorrowDate.AddDays(10);

                    // IsReserved = true if more reservations remain
                    IsReserved = Reservations.Count > 0;
               }
               else
               {
                    IsReserved = false;
               }

               // Successfully returned (and maybe re-borrowed)
               return true;
          }


          //Making a Reservation
          public void Reserve(DateTime reservationDate, string userName)
          {
               // Add reservation to the queue (Sorted Dictionary)
               Reservations.Add(reservationDate, userName);
               IsReserved = true;
           }

          //Updates User's name for a Specific Reservation Date
          public bool EditReservationName(DateTime reservationDate, string newName)
          {
               // Check if Reservation Exists
               if (Reservations.ContainsKey(reservationDate)) { 
                    Reservations[reservationDate] = newName;
                    return true;
               }
               return false;
          }

          //Updates Reservation for a Specific User
          public bool EditReservationDate(DateTime oldReservationDate, DateTime newReservationDate)
          {
               // Check if the old reservation exists, & new Resrvation is available
               if (Reservations.ContainsKey(oldReservationDate) && !Reservations.ContainsKey(newReservationDate))
               {
                    string reservedBy = Reservations[oldReservationDate];

                    // Remove the Old reservation
                    Reservations.Remove(oldReservationDate);

                    //Add the New Reservation
                    Reservations.Add(newReservationDate, reservedBy);

                    return true;
               }
               return false;
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
                    BorrowDate = BorrowDate,
                    ReturnDate = DateTime.Now,
                    FinePaid = CalculateFine()
               });
          }
     }
}
