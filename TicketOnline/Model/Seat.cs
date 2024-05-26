using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace TicketOnline.Data
{
    public class Seat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public string Id { get; set; }
        [Required]
        public char RowName { get; set; }
        [Required]
        public int SeatNumber { get; set; }
        [Required]
        public int RoomNumberId { get; set; }
        [ForeignKey("RoomNumberId")]
        public virtual Room? Room { get; set; }
        //one to many
        public virtual ICollection<Ticket>? Tickets { get; set; }
    }
}
