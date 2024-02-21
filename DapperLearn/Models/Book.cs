namespace DapperLearn.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Author> Authors { get; set; }
    }
}
