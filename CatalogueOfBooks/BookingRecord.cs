using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogueOfBooks
{
     public class BookingRecord
     {
          public string Borrower { get; set; }
          public DateTime BorrowDate { get; set; }
          public DateTime ReturnDate { get; set; }
          public decimal FinePaid { get; set; }
     }
}
