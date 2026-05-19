using Resend;

namespace Projeto360.Api.Services
{
    public class BasicSend
    {
        private readonly ResendClient _client;

        public BasicSend(ResendClient client)
        {
            _client = client;
        }

        public async Task SendEmailAsync(string email, int otp)
        {
            var message = new EmailMessage
            {
                From = "Acme <onboarding@resend.dev>",
                To = { email },
                Subject = "Codigo de Recuperação",
                HtmlBody = $"<strong>Codigo de Recuperação: {otp} expira em 15 minutos!</strong>",
            };

            await _client.EmailSendAsync(message);
        }
    }
}