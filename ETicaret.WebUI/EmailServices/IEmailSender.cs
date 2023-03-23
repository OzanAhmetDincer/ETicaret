namespace ETicaret.WebUI.EmailServices
{
    public interface IEmailSender
    {
        // smtp => gmail, hotmail
        // api  => sendgrid


        //Asenkron bir metot tanımlarız. Bu metot bir string email bilgisi alır: hangi mail adresine gidecek, string subject: giden mailin bir konusu olsun, string htmlMessage: html bir message oluşturulacak 
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
