using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketOnline.Data
{
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public string? Id { get; set; }
        [Required]
        public string OrderId { get; set; }
        //[ForeignKey("OrderId")]
        //public virtual Order? Order { get; set; }
        //[Required]
        public string ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
        public decimal Quantity { get; set; }

    }
}
