using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSA_Assign_2
{
    public static class LibraryService
    {
        public static void AddBook(List<Book> books)
        {
            Console.Clear();
            Console.WriteLine("=== Add New Book ===");
            Console.Write("Enter ISBN: ");
            string isbn = Console.ReadLine();

            Console.Write("Enter Title: ");
            string title = Console.ReadLine();

            Console.Write("Enter Publisher: ");
            string publisher = Console.ReadLine();

            Console.Write("Enter Genre: ");
            string genre = Console.ReadLine();

            Console.Write("Enter Language: ");
            string language = Console.ReadLine();

            Console.Write("Enter Authors (comma separated): ");
            List<string> authors = Console.ReadLine().Split(',').Select(a => a.Trim()).ToList();

            Console.Write("Enter Publication Date (dd-MM-yyyy): ");
            DateTime publicationDate = DateTime.ParseExact(Console.ReadLine(), "dd-MM-yyyy", null);

            Book book = new Book(isbn, title, publicationDate, publisher, authors, genre, language);
            books.Add(book);

            Console.WriteLine("Book added successfully!");
            Console.WriteLine("Press Enter to return to the menu.");
            Console.ReadLine();
        }

        public static void ViewBooks(List<Book> books)
        {
            Console.Clear();
            Console.WriteLine("=== All Books ===");
            foreach (var book in books)
            {
                Console.WriteLine("-----------------------------------");
                Console.WriteLine(book);
            }

            Console.WriteLine("\nPress Enter to return to the menu.");
            Console.ReadLine();
        }

        public static void RemoveBook(List<Book> books)
        {
            Console.Clear();
            Console.Write("Enter ISBN of the book to remove: ");
            string isbn = Console.ReadLine();

            Book book = books.FirstOrDefault(b => b.ISBN == isbn);
            if (book != null)
            {
                books.Remove(book);
                Console.WriteLine("Book removed successfully!");
            }
            else
            {
                Console.WriteLine("Book not found!");
            }

            Console.WriteLine("\nPress Enter to return to the menu.");
            Console.ReadLine();
        }

        public static void SearchBook(List<Book> books)
        {
            Console.Clear();
            Console.WriteLine("=== Search Book ===");
            Console.Write("Enter search term (Title/Author/Year): ");
            string searchTerm = Console.ReadLine();

            var searchResults = books.Where(b =>
    b.Title.ToLower().Contains(searchTerm.ToLower()) ||
    b.Authors.Any(a => a.ToLower().Contains(searchTerm.ToLower())) ||
    b.DateOfPublication.Year.ToString().Contains(searchTerm)
).ToList();


            if (searchResults.Any())
            {
                foreach (var book in searchResults)
                {
                    Console.WriteLine("-----------------------------------");
                    Console.WriteLine(book);
                }
            }
            else
            {
                Console.WriteLine("No books found.");
            }

            Console.WriteLine("\nPress Enter to return to the menu.");
            Console.ReadLine();
        }

        public static void EditBookDetails(List<Book> books)
        {
            Console.Clear();
            Console.Write("Enter ISBN of the book to edit: ");
            string isbn = Console.ReadLine();

            Book book = books.FirstOrDefault(b => b.ISBN == isbn);
            if (book != null)
            {
                Console.Write("Enter new Title (leave blank to keep current): ");
                string title = Console.ReadLine();
                if (!string.IsNullOrEmpty(title)) book.Title = title;

                Console.Write("Enter new Publisher (leave blank to keep current): ");
                string publisher = Console.ReadLine();
                if (!string.IsNullOrEmpty(publisher)) book.Publisher = publisher;

                Console.Write("Enter new Genre (leave blank to keep current): ");
                string genre = Console.ReadLine();
                if (!string.IsNullOrEmpty(genre)) book.Genre = genre;

                Console.Write("Enter new Language (leave blank to keep current): ");
                string language = Console.ReadLine();
                if (!string.IsNullOrEmpty(language)) book.Language = language;

                Console.Write("Enter new Authors (comma separated, leave blank to keep current): ");
                string authorsInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(authorsInput))
                    book.Authors = authorsInput.Split(',').Select(a => a.Trim()).ToList();

                Console.Write("Enter new Publication Date (dd-MM-yyyy, leave blank to keep current): ");
                string dateInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(dateInput))
                    book.DateOfPublication = DateTime.ParseExact(dateInput, "dd-MM-yyyy", null);

                Console.WriteLine("Book details updated successfully!");
            }
            else
            {
                Console.WriteLine("Book not found!");
            }

            Console.WriteLine("\nPress Enter to return to the menu.");
            Console.ReadLine();
        }

        public static void BorrowBook(List<Book> books)
        {
            Console.Clear();
            Console.Write("Enter ISBN of the book to borrow: ");
            string isbn = Console.ReadLine();

            Book book = books.FirstOrDefault(b => b.ISBN == isbn);
            if (book != null)
            {
                if (book.IsBorrowed)
                {
                    Console.WriteLine("The book is already borrowed. Do you want to reserve it? (y/n): ");
                    string response = Console.ReadLine().ToLower();
                    if (response == "y")
                    {
                        Console.Write("Enter details of user: ");
                        string librarianName = Console.ReadLine();
                        Console.Write("Enter user ID: ");
                        string librarianId = Console.ReadLine();
                        book.ReservationQueue.Enqueue((librarianName, librarianId));
                        Console.WriteLine("Book reserved successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Reservation cancelled.");
                    }
                }
                else
                {
                    Console.Write("Enter details of user: ");
                    string borrowerName = Console.ReadLine();

                    Console.Write("Enter user ID: ");
                    string borrowerId = Console.ReadLine();

                    book.IsBorrowed = true;
                    book.BorrowedByUsername = borrowerName;
                    book.BorrowedByUserId = borrowerId;
                    book.DateOfIssue = DateTime.Now;

                    book.BorrowHistory.Add(new BorrowRecord(borrowerName, borrowerId, DateTime.Now));

                    Console.WriteLine("Book borrowed successfully!");
                }
            }
            else
            {
                Console.WriteLine("Book not found!");
            }

            Console.WriteLine("\nPress Enter to return to the menu.");
            Console.ReadLine();
        }

        public static void ReturnBook(List<Book> books)
        {
            Console.Clear();
            Console.Write("Enter ISBN of the book to return: ");
            string isbn = Console.ReadLine();

            Book book = books.FirstOrDefault(b => b.ISBN == isbn);
            if (book != null)
            {
                if (book.IsBorrowed)
                {
                    double fine = book.CalculateFine();
                    if (fine > 0)
                    {
                        Console.WriteLine($"The fine for returning the book late is: ${fine:F2}");
                    }

                    book.IsBorrowed = false;
                    book.BorrowedByUsername = "";
                    book.BorrowedByUserId = "";
                    book.DateOfIssue = null;

                    if (book.ReservationQueue.Any())
                    {
                        var reserved = book.ReservationQueue.Dequeue();
                        Console.WriteLine($"The book is now reserved for: {reserved.username} (ID: {reserved.userId})");
                    }
                    else
                    {
                        Console.WriteLine("Book returned. No reservations pending.");
                    }
                }
                else
                {
                    Console.WriteLine("This book is not currently borrowed.");
                }
            }
            else
            {
                Console.WriteLine("Book not found!");
            }

            Console.WriteLine("\nPress Enter to return to the menu.");
            Console.ReadLine();
        }

        public static void ViewSortedBooks(List<Book> books)
        {
            var sortedBooks = books.OrderBy(b => b.DateOfPublication).ThenBy(b => b.Title).ToList();
            Console.Clear();
            Console.WriteLine("=== Sorted Books (By Year → Title) ===");
            foreach (var book in sortedBooks)
            {
                Console.WriteLine("-----------------------------------");
                Console.WriteLine(book);
            }

            Console.WriteLine("\nPress Enter to return to the menu.");
            Console.ReadLine();
        }

        public static void ViewBorrowedBooks(List<Book> books)
        {
            Console.Clear();
            Console.WriteLine("=== Borrowed Books ===");
            var borrowedBooks = books.Where(b => b.IsBorrowed).ToList();

            if (borrowedBooks.Any())
            {
                foreach (var book in borrowedBooks)
                {
                    Console.WriteLine("-----------------------------------");
                    Console.WriteLine(book);
                    Console.WriteLine($"Borrowed By: {book.BorrowedByUsername} (ID: {book.BorrowedByUserId})");
                    Console.WriteLine($"Date of Issue: {book.DateOfIssue:dd-MM-yyyy}");
                }
            }
            else
            {
                Console.WriteLine("No books are currently borrowed.");
            }

            Console.WriteLine("\nPress Enter to return to the menu.");
            Console.ReadLine();
        }

        public static void ViewReservedBooks(List<Book> books)
        {
            Console.Clear();
            Console.WriteLine("=== Reserved Books ===");
            var reservedBooks = books.Where(b => b.ReservationQueue.Any()).ToList();

            if (reservedBooks.Any())
            {
                foreach (var book in reservedBooks)
                {
                    Console.WriteLine("-----------------------------------");
                    Console.WriteLine(book);
                    Console.WriteLine("Reservation Queue:");
                    foreach (var reservation in book.ReservationQueue)
                    {
                        Console.WriteLine($"- {reservation.username} (ID: {reservation.userId})");
                    }
                }
            }
            else
            {
                Console.WriteLine("No books are currently reserved.");
            }

            Console.WriteLine("\nPress Enter to return to the menu.");
            Console.ReadLine();
        }
    }
}

