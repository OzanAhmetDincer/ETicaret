using System.Net.Mail;
using System.Net;

namespace ETicaret.WebUI.EmailServices
{
    public class SmtpEmailSender : IEmailSender
    {
        // Aşağıda bir consractor oluştururuz. SmtpEmailSender'a dışarıdan gelecek olan parametreleri yazarız. string host: serverin bir adresi olacak, int port: hangi port üzerinden bu bilgiyi göndericez, bool enableSSL: mail şifreleme olayını true yada false olarak gönderebiliriz, string username: kullanacağımız mail adresi bizim kullanacağımız username'mimiz olacak, string password: hesabımıza girerken kullandığımız password bilgisi.
        private string _host;
        private int _port;
        private bool _enableSSL;
        private string _username;
        private string _password;
        public SmtpEmailSender(string host, int port, bool enableSSL, string username, string password)
        {
            this._host = host;
            this._port = port;
            this._enableSSL = enableSSL;
            this._username = username;
            this._password = password;
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient(this._host, this._port)
            {
                Credentials = new NetworkCredential(_username, _password),
                EnableSsl = this._enableSSL
            };

            return client.SendMailAsync(
                new MailMessage(this._username, email, subject, htmlMessage)
                {
                    IsBodyHtml = true// Html şeklinde bu mesajı gönderdiğimizi söylüyoruz
                }
            );
        }

        /* Buradaki işlemleri kullanabilmek içinde appsettings.json dosyası içerisinde tanımlamalar yapmamız lazım. EmailSerder isminde bir özellik tanımlarız bu içerisine kullanacak olduğum mail serverinin bilgilerini yazacaz. Host bilgisi ekleriz hotmail email service diyince internetten bilgi edinebiliriz. Bu hostun Port bilgiside aratınca çıkıyor, EnableSSL true derizki bize şifrelenmiş olarak gelsin, UserName bilgisi kendi kullanacağımız mail hesabı, Password bilgisi kendi parola bilgim. Bunları uygulama içerisinden çağırıp göndericez. office365.com hostu hotmail'in. Başka bir zaman gmail yada satın aldığın hosting firmasının smtp serverini kullanmak istersek "EmailSender" altında tanımladığımız bilgileri değiştiririz. */
    }
}
