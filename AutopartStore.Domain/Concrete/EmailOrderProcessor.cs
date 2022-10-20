using System.Net;
using System.Net.Mail;
using System.Text;
using AutopartStore.Domain.Abstract;
using AutopartStore.Domain.Entities;

namespace AutopartStore.Domain.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress = "orders@example.com";
        public string MailFromAddress = "autopartstore@example.com";
        public bool UseSsl = true;
        public string Username = "MySmtpUsername";
        public string Password = "MySmtpPassword";
        public string ServerName = "smtp.example.com";
        public int ServerPort = 587;
        public bool WriteAsFile = true;
        public string FileLocation = @"d:\Study\game_store_emails";
    }
    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;
        public EmailOrderProcessor(EmailSettings settings)
        {
            emailSettings = settings;
        }
        public void ProcessOrder(Cart cart, ShippingDetails shippingInfo)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials
                    = new NetworkCredential(emailSettings.Username, emailSettings.Password);

                if (emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod
                        = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                StringBuilder body = new StringBuilder()
                    .AppendLine("Новый заказ обработан")
                    .AppendLine("-------")
                    .AppendLine("Товары:");

                foreach (var line in cart.Lines)
                {
                    var subtotal = line.Autopart.Price * line.Quantity;
                    body.AppendFormat("{0} x {1} (итого: {2:c})\n",
                        line.Quantity, line.Autopart.Name, subtotal);
                }

                body.AppendFormat("\nОбщая стоимость: {0:c}", cart.ComputeTotalValue())
                    .AppendLine("")
                    .AppendLine("-------")
                    .AppendLine("Доставка:")
                    .AppendLine(shippingInfo.Name)
                    .AppendLine(shippingInfo.Number)
                    .AppendLine(shippingInfo.Line1)
                    .AppendLine(shippingInfo.City)
                    .AppendLine(shippingInfo.Country)
                    .AppendLine("-------")
                    .AppendFormat("Способ доставки: {0}",
                        shippingInfo.DeliveryMethod ? "Курьер" : "Самовывоз");

                MailMessage mailMessage = new MailMessage(
                                       emailSettings.MailFromAddress,	// От кого
                                       emailSettings.MailToAddress,		// Кому
                                       "Новый заказ отправлен!",		// Тема
                                       body.ToString()); 				// Тело письма

                if (emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.UTF8;
                }

                smtpClient.Send(mailMessage);
            }
        }
    }
}
