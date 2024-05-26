using TicketOnline.Data;

namespace TicketOnline.Model
{
    public class PlaceOrder
    {
        public virtual ICollection<ProductAdd>? Products { get; set; }
        public virtual TicketOrder Tickets { get; set; }
    }
}
