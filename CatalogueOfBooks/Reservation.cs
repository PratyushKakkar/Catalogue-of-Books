using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogueOfBooks
{
     public class Reservation
     {
          public string ReservedBy { get; }      // User Name
          public DateTime ReservedDate { get; }  // When the reservation was made

          public Reservation(string reservedBy, DateTime reservedDate)
          {
               ReservedBy = reservedBy;
               ReservedDate = reservedDate;
          }
     }
}
