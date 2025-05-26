using System.Net;
using System.Net.Sockets;
using System.Text;
using Server.Services;

var server = new TcpListener(IPAddress.Loopback, 5000);
server.Start();
Console.WriteLine("Server initiated at localhost:5000");

var bookService = new BookService(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Books"));

while (true)
{
    var client = server.AcceptTcpClient();
    _ = Task.Run(() => HandleClient(client, bookService));
}

static void HandleClient(TcpClient client, BookService bookService)
{
    using var stream = client.GetStream();
    using var reader = new StreamReader(stream, Encoding.UTF8);
    using var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

    try
    {
        while (true)
        {
            string? searchTerm = reader.ReadLine();
            if (searchTerm == null) break;

            if (searchTerm.ToLower() == "q")
            {
                Console.WriteLine("Client disconnected.");
                break;
            }

            Console.WriteLine($"Search received: {searchTerm}");

            var foundBooks = bookService.SearchBooks(searchTerm);

            if (foundBooks.Count == 0)
            {
                writer.WriteLine("No books found.");
                continue;
            }

            if (foundBooks.Count == 1)
            {
                var book = foundBooks[0];
                writer.WriteLine($"Opening: {book}");
                OpenFile(book.path);
                continue;
            }

            writer.WriteLine("Several books found:");
            for (int i = 0; i < foundBooks.Count; i++)
            {
                writer.WriteLine($"{i + 1} - {foundBooks[i]}");
            }
            writer.WriteLine("Enter the corresponding number:");

            string choice = reader.ReadLine() ?? "";
            if (int.TryParse(choice, out int index) &&
                index >= 1 && index <= foundBooks.Count)
            {
                var selectedBook = foundBooks[index - 1];
                writer.WriteLine($"Opening: {selectedBook}");
                OpenFile(selectedBook.path);
            }
            else
            {
                writer.WriteLine("Invalid choice.");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}

static void OpenFile(string path)
{
    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
    {
        FileName = path,
        UseShellExecute = true
    });
}
