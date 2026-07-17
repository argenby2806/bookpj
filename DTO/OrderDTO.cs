namespace bookpj.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime OrderDate { get; set; }
        public string TrackingCode { get; set; } = string.Empty;
        public List<DetailOrderDTO> DetailOrders { get; set; } = new List<DetailOrderDTO>();
    }
}
