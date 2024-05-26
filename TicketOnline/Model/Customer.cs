using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketOnline.Data
{

    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [DataType(DataType.Date)]
        public DateOnly Dob { get; set; } = DateOnly.Parse("1/1/2000");
        [EmailAddress]
        public string Email { get; set; }
        public bool EmailConfirm { get; set; } = false;
        [Phone]
        public string PhoneNumber { get; set; }
        public bool PhoneConfirm { get; set; } = false;
        public decimal Point { get; set; } = 0;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public DateTime? CreateAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdateAt { get; set; }
        //one to many

    }
}
