using System.ComponentModel.DataAnnotations;

namespace TicketOnline.Data
{
    public class Room
    {
        [Key]
        public int RoomNumber { get; set; }
        public ICollection<Seat>? Seats { get; set; }
        public ICollection<ShowTime>? ShowTimes { get; set; }
    }
}
