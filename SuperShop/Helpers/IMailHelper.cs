﻿namespace SuperShop.Helpers
{
    // Interface para o serviço de envio de emails
    public interface IMailHelper
    {
        // Método para enviar um email. Recebe três parâmetros:
        // 'to' - o endereço de email do destinatário,
        // 'subject' - o assunto do email,
        // 'body' - o corpo da mensagem.
        Response SendEmail(string to, string subject, string body);
    }
}
