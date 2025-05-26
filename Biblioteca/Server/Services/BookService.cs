using UglyToad.PdfPig;
using Server.Models;

namespace Server.Services;

public class BookService
{
    private readonly string _booksDirectory;

    public BookService(string booksDirectory)
    {
        _booksDirectory = booksDirectory;
    }

    public List<Book> SearchBooks(string term)
    {
        var books = new List<Book>();
        var files = Directory.GetFiles(_booksDirectory, "*.pdf");

        Parallel.ForEach(files, file =>
        {
            var book = ReadBook(file);
            if (book != null &&
                (book.title.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                 book.author.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                 book.content.Contains(term, StringComparison.OrdinalIgnoreCase)))
            {
                lock (books)
                {
                    books.Add(book);
                }
            }
        });

        return books;
    }

    public Book? ReadBook(string path)
    {
        try
        {
            using var document = PdfDocument.Open(path);
            var info = document.Information;

            string title = !string.IsNullOrWhiteSpace(info.Title)
                ? info.Title
                : Path.GetFileNameWithoutExtension(path);

            string author = !string.IsNullOrWhiteSpace(info.Author)
                ? info.Author
                : "Unknown";

            string content = "";
            foreach (var page in document.GetPages())
            {
                content += page.Text + " ";
                if (content.Length > 2000) break;
            }

            return new Book(path, title, author, content);
        }
        catch
        {
            return new Book(path, Path.GetFileNameWithoutExtension(path), "Unknown", "");
        }
    }
}
