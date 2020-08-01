using MimeKit;
using System;
using System.Threading.Tasks;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;


namespace EmailConfig
{
    public class GmailSender : IMessage
    {
        private readonly EmailConfiguration _emailConfiguration;

        public GmailSender(EmailConfiguration emailConfiguration) {
            _emailConfiguration = emailConfiguration;
        }

        public async Task SendMailAsync(Message message)
        {
            var emailMessage = createEmailMessage(message);
            await SendAsync(emailMessage);
        }

        private MimeMessage createEmailMessage(Message message) {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfiguration.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

            return emailMessage;


        }

        private async Task SendAsync(MimeMessage mailMessage) {
            using (var client = new SmtpClient()) {

                try
                {
                    client.CheckCertificateRevocation = false;

                    await client.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_emailConfiguration.Username, _emailConfiguration.Password);
                    await client.SendAsync(mailMessage);

                }
                catch(Exception e)
                {


                }
                finally {
                    await client.DisconnectAsync(true);
                    client.Dispose();

                }
                

            }

        }
    }
}
