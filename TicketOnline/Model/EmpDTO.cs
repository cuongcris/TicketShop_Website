using System.ComponentModel.DataAnnotations;

namespace TicketOnline.Model
{
    public class EmpDTO
    {
        public string? Cid { get; set; }
        public string? Name { get; set; }
        public string? Dob { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Position { get; set; }
        public string? StartDate { get; set; }
    }
}
