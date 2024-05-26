using TicketOnline.Data;

namespace TicketOnline.Model
{
    public class TicketOrder
    {
        public string ShowTimeId { get; set; }
        public virtual ICollection<TicketAdd> Tickets { get; set; }
    }
}
