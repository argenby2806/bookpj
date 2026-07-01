namespace bookpj.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime BorrowedAT { get; set; }
        public DateTime DueDate { get; set; }
    }
}
