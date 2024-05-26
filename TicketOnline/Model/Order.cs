using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlTypes;

namespace TicketOnline.Data
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public string? Id { get; set; }
        [Required]
        public string CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }
        [DataType(DataType.Currency)]
        public decimal Total { get; set; } = 0;
        public bool Status { get; set; }
        public DateTime? CreateAt { get; set; } = DateTime.UtcNow;
        //one to many
        public virtual ICollection<OrderItem>? OrderItems { get; set; }
        public virtual ICollection<Ticket>? Tickets { get; set; }
    }
}
