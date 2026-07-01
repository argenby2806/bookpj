namespace bookpj.DTO
{
    public class UpdateBookDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime BorrowedAT { get; set; }
        public DateTime DueDate { get; set; }
    }
}
