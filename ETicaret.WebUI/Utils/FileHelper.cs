namespace ETicaret.WebUI.Utils
{
    public class FileHelper
    {
        /* Aşağıdaki kodda public dedik her yerden erişilsin diye, static, programımız çalışır çalışmaz bu classı çalıştırması için yazdık. İçinde buluduğu sınıftan nesne oluşturulmadan veya hiç bir nesneye referans olmadan kullanılabilen üyeler static olarak nitelendirilir. Metotlar ve alanlar static olarak tanımlanabilir. herhangi bir nesne oluşturulmadan önce çağırılması gerektiği için static olarak tanımlanmıştır. Başka bir deyişle de bir nesne metodun üreteceği sonucu etkilemeyecek ise o metot static olarak tanımlanır. Static olarak tanımlanan bir metoda program çalıştığı sürece erişilir, böylece sadece bir metot ile birden çok nesne çağırılır. async, asenkron olarak çalışcağını gösterir. Resim yükleme işlemini bir çok yerde yapacağımız için bunu bir metot olarak bir class içinde tanımladık. IFormFile resim yüklemek için kullanılan bir interface. formFile ile yükleme yapacağımız resmin ismini aldık. string filePath = "/wwwroot/Img/" : bu kod ile yükleyeceğimiz resimin nereye kayıt edileceğini gösterdik. */
        public static async Task<string> FileLoaderAsync(IFormFile formFile, string filePath )
        {
            // string filePath' e bir başlangıç değeri atarız, eğer bir değer gelmezse varsayılan olarak ana dizinde "Img" klasörüne yüklensin dedik.
            var fileName = "";// fileName adında boş bir nesne oluşturduk.
            if (formFile != null && formFile.Length > 0)// eğer formFile null değilse ve uzunluğu(lenght) sıfırdan büyük ise yani gelen dosyanın içerisinde bir değer varsa aşağıdaki kodları yap
            {
                fileName = formFile.FileName;// FileName ile formFile adında yüklenen ismini, boş olarak oluşturduğumuz fileName içine at.
                string directory = Directory.GetCurrentDirectory() + "/wwwroot" + filePath + fileName;// Directory bir classtır ve bu class içindeki GetCurrentDirectory metodu ile yükleyeceğimiz dosyanın yolunu alıyoruz yani hangi dizinde çalışıyorsa filePath ile projede nereye kayıt edeceğinin ismini alıyoruz, fileName ile yüklenecek dosyanın ismini alıp bu hepsini birleştirip string olan directory nesnesinin içine atıyoruz.
                using (var stream = new FileStream(directory , FileMode.Create))// stream isminde bir nesne oluşturup FileStream classını kullanarak directory içerisindeki yüklenen dosyamızın tam ismiyle beraber,FileMode.Create ile dosyayı stream nesnesini oluşturmak.
                {
                    await formFile.CopyToAsync(stream);// asenkron bir şekilde yüklenen stream nesnesini formFile a aktardıkk. fileName içerisinde formFile nesnesinin adı vardı zaten bunuda return ile geri döndürdük.
                }
            }
            return fileName;
        }
        public static bool FileRemover(string fileName, string filePath)
        {
            string directory = Directory.GetCurrentDirectory() + "/wwwroot" + filePath + fileName;
            if (File.Exists(directory))// "File.Exists" dosya var mı yok mu onu kontrol eder
            {
                // .net de dosya ve klasör işlemleri için File sınıfı mevcuttur. File.Exists metodu kendisine verilen yolda(directory) bir dosya var mı yok mu kontrol eder. Dosya varsa true yoksa false bilgisini getirir.
                File.Delete(directory);// eğer klasörde dosya varsa dosyayı fiziksel olarak sil. Bu işlemi yapmadan sildiğimizde hafızada yüklenen dosya kalacağından boşa yer kaplayacak.
                return true;// silme başarılıysa geriye true dön ki metodu kullandığımız yerde işlemin başarılı olduğunu bilelim.
            }
            return false;
        }

        internal static Task<string?> FileLoaderAsync(IFormFile? logo)
        {
            throw new NotImplementedException();
        }
    }
}
