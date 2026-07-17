using Microsoft.AspNetCore.Identity;

namespace bookpj.Entities
{
    public class Order
    {
        public int id { get; set; }
        public string UserName { get; set; }
        public IdentityUser? User { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string TrackingCode { get; set; } = string.Empty;
        public string UserId { get; set; }
        public List<DetailOrder> DetailOrders { get; set; } = new List<DetailOrder>();
    }
}
