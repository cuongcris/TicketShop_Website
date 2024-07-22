using System.Net;
using System.Net.Mail;
using System.Text;
using TicketOnline.Data;

public class EmailSender
{
    public void SendOrderConfirmationEmail(string recipientEmail, Order order, List<Ticket> tickets, List<OrderItem> orderItems, decimal totalAmount)
    {
        try
        {
            // Tạo nội dung email dưới dạng HTML
            StringBuilder body = new StringBuilder();
            body.AppendLine("<html>");
            body.AppendLine("<body>");
            body.AppendLine($"<h2>Order Confirmation: OrderCode - {order.Id}</h2>");
            body.AppendLine("<p><strong>Tickets:</strong></p>");
            body.AppendLine("<ul>");
            foreach (var ticket in tickets)
            {
                body.AppendLine($"<li>Ticket for Seat {ticket.SeatId} - Price: {ticket.Price} VND</li>");
            }
            body.AppendLine("</ul>");

            if (orderItems.Any())
            {
                body.AppendLine("<p><strong>Order Items:</strong></p>");
                body.AppendLine("<ul>");
                foreach (var orderItem in orderItems)
                {
                    body.AppendLine($"<li>{orderItem.Quantity} x {orderItem.Product.Name}</li>");
                }
                body.AppendLine("</ul>");
            }

            body.AppendLine($"<p><strong>Total Amount:</strong> ${totalAmount.ToString("F2")}</p>");
            body.AppendLine("</body>");
            body.AppendLine("</html>");

            // Cấu hình thông tin mail
            string fromEmail = "hoangbig1@gmail.com"; // Email người gửi
            string password = "jxuibgdogvmixpvo"; // Mật khẩu email người gửi
            string subject = "Order Confirmation"; // Tiêu đề email
            string fromName = "Cinematic"; // Tên hiển thị

            // Tạo đối tượng MailAddress với tên người gửi
            MailAddress fromAddress = new MailAddress(fromEmail, fromName);

            MailMessage message = new MailMessage();
            message.From = fromAddress;
            message.To.Add(recipientEmail);
            message.Subject = subject;
            message.Body = body.ToString();
            message.IsBodyHtml = true;

            // Cấu hình thông tin SMTP server
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential(fromEmail, password);
            smtpClient.EnableSsl = true;

            // Gửi email
            smtpClient.Send(message);

        }
        catch (Exception ex)
        {
            throw;
        }
    }
}