using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSA_Assign_2
{
    public class Book
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public DateTime DateOfPublication { get; set; }
        public string Publisher { get; set; }
        public List<string> Authors { get; set; }
        public string Genre { get; set; }
        public string Language { get; set; }
        public bool IsBorrowed { get; set; }
        public string BorrowedByUsername { get; set; }
        public string BorrowedByUserId { get; set; }
        public DateTime? DateOfIssue { get; set; }
        public Queue<(string username, string userId)> ReservationQueue { get; set; }
        public List<BorrowRecord> BorrowHistory { get; set; }

        public Book(string isbn, string title, DateTime dateOfPublication, string publisher, List<string> authors, string genre, string language)
        {
            ISBN = isbn;
            Title = title;
            DateOfPublication = dateOfPublication;
            Publisher = publisher;
            Authors = authors;
            Genre = genre;
            Language = language;
            IsBorrowed = false;
            BorrowedByUsername = "";
            BorrowedByUserId = "";
            DateOfIssue = null;
            ReservationQueue = new Queue<(string username, string userId)>();
            BorrowHistory = new List<BorrowRecord>();
        }

        public double CalculateFine()
        {
            if (!DateOfIssue.HasValue)
                return 0;

            int overdueDays = (DateTime.Now - DateOfIssue.Value).Days - 10;

            return overdueDays > 0 ? overdueDays * 0.50 : 0;
        }

        public override string ToString()
        {
            return $"{ISBN}\nTitle: {Title}\nPublisher: {Publisher}\nGenre: {Genre}\nLanguage: {Language}\n" +
                   $"Authors: {string.Join(", ", Authors)}\nPublication Date: {DateOfPublication:dd-MM-yyyy}";
        }
    }

    public class BorrowRecord
    {
        public string Username { get; set; }
        public string UserId { get; set; }
        public DateTime DateOfIssue { get; set; }

        public BorrowRecord(string username, string userId, DateTime dateOfIssue)
        {
            Username = username;
            UserId = userId;
            DateOfIssue = dateOfIssue;
        }
    }
}

