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

          public bool IsBorrowed { get; set; }                        //bool to store if the book is borrowed
          public string BorrowedBy { get; set; }                    //Person who Borrowed the book
          public DateTime BorrowDate { get; set; }               //Date when the book was borrowed
          public DateTime DueDate { get; set; }                  //Due date for returning the book

          public bool IsReserved { get; set; }                        //bool to store if the book is reserved
          public Queue<Reservation> ReservationQueue { get; } = new Queue<Reservation>();


          // Constructor
          public Book(string isbn,
                 string title,
                 DateTime dateOfPublication,
                 string publisher,
                 List<string> authors,
                 string genre,
                 string language,
                 string description,
                 decimal price )
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
          }

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

          public bool Return()
          {
               // Book is not borrowed 
               if (!IsBorrowed)
                    return false;

               //if Reservations exist (someone is in Queue to borrow the book)
               if (ReservationQueue.Count > 0)
               {
                    Reservation r = ReservationQueue.Dequeue();
                    IsReserved = (ReservationQueue.Count>0) ? true : false;

                    //Move Reservation to Borrowing
                    IsBorrowed = true;
                    BorrowedBy = r.ReservedBy;
                    BorrowDate = r.ReservedDate;
                    DueDate = BorrowDate.AddDays(10);  
               }
               else
               {
                    IsReserved = false;
                    IsBorrowed = false;
                    BorrowedBy = null;
                    BorrowDate = DateTime.MinValue;
               }

               // Successfully Returned book
               return true;
          }

          public void Reserve(string user, DateTime reservedDate)
          {
               // Add reservation to the queue
               ReservationQueue.Enqueue(new Reservation(user, reservedDate));
               IsReserved = true;
           }

          public decimal CalculateFine()
          {
               // Calculate fine if the book is overdue
               if (IsBorrowed && (DateTime.Now > DueDate))
               {
                    TimeSpan overdueDays = DateTime.Now - DueDate;
                    return (decimal)overdueDays.TotalDays * 0.50m; // 0.50 per day
               }

               // No fine if not overdue or not borrowed
               return 0.0m;
          }
     }
}
