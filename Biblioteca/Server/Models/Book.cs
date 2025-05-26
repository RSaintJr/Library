namespace Server.Models
{
    public class Book
    {
        public string path { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public string content { get; set; }

        public Book(string path, string title, string author, string content)
        {
            this.path = path;
            this.title = title;
            this.author = author;
            this.content = content;
        }

        public override string ToString()
        {
            return $"{title} - {author}";
        }
    }

}
