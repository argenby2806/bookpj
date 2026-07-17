using bookpj.Entities;

namespace bookpj.DTO
{
    public class UpdateOrderDTO
    {
        public string UserName { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public List<UpdateDetailOrderDTO> DetailOrders { get; set; } = new List<UpdateDetailOrderDTO>();
    }
}
