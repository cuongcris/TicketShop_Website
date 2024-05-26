using TicketOnline.Data;

namespace TicketOnline.Model
{
    public class TicketGet
    {
        public string Id { get; set; }
        public Seat Seat { get; set; }
        public Order Order { get; set; }
        public ShowTime ShowTime { get; set; }
    }
}
