using TicketOnline.Data;

namespace TicketOnline.Model
{
    public class LoginResponse
    {
        public LoginResponse(Customer user, string token)
        {
            this.user = user;
            this.token = token;
        }

        public Customer user { get; set; }

        public string token { get; set; }
    }
}
