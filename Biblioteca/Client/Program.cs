using System.Net.Sockets;
using System.Text;

using var client = new TcpClient("127.0.0.1", 5000);
using var stream = client.GetStream();
using var reader = new StreamReader(stream, Encoding.UTF8);
using var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

Console.WriteLine("Connected to server.");

while (true)
{
    Console.WriteLine("\nEnter the search term (or 'q' to quit):");
    string search = Console.ReadLine() ?? "";

    if (search.ToLower() == "q")
    {
        Console.WriteLine("Disconnecting from server...");
        break;
    }

    writer.WriteLine(search);

    while (true)
    {
        string? response = reader.ReadLine();
        if (response == null) break;

        Console.WriteLine(response);

        if (response.Contains("Enter the corresponding number:"))
        {
            string choice = Console.ReadLine() ?? "";
            writer.WriteLine(choice);
        }

        if (response.StartsWith("Opening:") ||
            response.StartsWith("No books found.") ||
            response.StartsWith("Invalid choice."))
        {
            break;
        }
    }
}
