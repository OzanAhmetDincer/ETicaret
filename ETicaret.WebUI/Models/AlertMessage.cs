namespace ETicaret.WebUI.Models
{
    public class AlertMessage
    {
        public string Title { get; set; }// AlertMessage'nin başlık kısmı
        public string Message { get; set; }// AlertMessage'nin body kısmı
        public string AlertType { get; set; }// AlertMessage'nin css kısmı
    }
}
