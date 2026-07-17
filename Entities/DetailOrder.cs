namespace bookpj.Entities
{
    public class DetailOrder
    {
        public int id { get; set; }
        public string Title { get; set; } 
        public string Author { get; set; } 
        public decimal Price { get; set; }
        public string OrderTrackingCode { get; set; } = string.Empty;
        public Order Order { get; set; }

    }
}
