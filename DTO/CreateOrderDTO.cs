namespace bookpj.DTO
{
    public class CreateOrderDTO
    {
        public string UserName { get; set; }
        public DateTime? OrderDate { get;set; } = DateTime.Now;
        public List<CreateDetailOrderDTO> DetailOrders { get; set; } = new List<CreateDetailOrderDTO>();
    }
}
