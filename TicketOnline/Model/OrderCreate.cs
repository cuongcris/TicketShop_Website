using System.ComponentModel.DataAnnotations;

namespace TicketOnline.Model
{
    public class OrderCreate
    {
        public string CustomerId { get; set; }
        public decimal Total { get; set; } = 0;
        public bool Status { get; set; }
        public DateTime? CreateAt { get; set; } = DateTime.UtcNow;
    }
}
