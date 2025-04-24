using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSA_Assign_2
{
    public class CatalogueOfBooks
    {
        private string filePath = "books.txt";

        public List<Book> LoadBooks()
        {
            List<Book> books = new List<Book>();

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var details = line.Split(',');
                    var isbn = details[0];
                    var title = details[1];
                    var dateOfPublication = DateTime.ParseExact(details[2], "dd-MM-yyyy", null);
                    var publisher = details[3];
                    var authors = details[4].Split('|').ToList();
                    var genre = details[5];
                    var language = details[6];

                    books.Add(new Book(isbn, title, dateOfPublication, publisher, authors, genre, language));
                }
            }

            return books;
        }

        public void SaveBooks(List<Book> books)
        {
            var lines = books.Select(book =>
                $"{book.ISBN},{book.Title},{book.DateOfPublication:dd-MM-yyyy},{book.Publisher}," +
                $"{string.Join("|", book.Authors)},{book.Genre},{book.Language}");

            File.WriteAllLines(filePath, lines);
        }
    }
}

