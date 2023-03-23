using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace ETicaret.WebUI.Extensions
{
    public static class TempDataExtensions
    {
        // Controller tarafından yapılan işlemler sonucunda belirli mesajları ekrana aktarırız. AlertMessage, modelstate vb kullanarak bu işlemleri yaptık. Biz mesajlarımızı TempData ile ekrana getirebiliriz. Biz aşağıdaki kodlarla TempData'yı ExtensionMetot yardımı ile genişletiyoruz. Düzensiz olarak sayfa üzerine gönderdiğimiz mesajları hem düzenlemiş oluruz hemde daha çok işlem yapabiliriz. Komplex tipleri(fazla bilgi içeren) direkt tempdata içerisine atamıyoruz. Bir serialize işlemi yapmamız gerek.
        // Oluşturulacak Extension metot(TempDataExtensions) static olması gerekiyor. Geri dönüş değeri olmayacak(void), içerisinde bir put ve get metodu olacak. Yani tempdata içerisine bir bilgi eklerken serilize edip o şekilde bilgiyi ekliyecez. Serilize edeceğimiz bilginin tipi "T" yani generic olsun, this ile bunu hangi tip üzerinden çağıracağımızı belirtecez(yani tempdata üzerine geldiğimizde "Microsoft.AspNetCore......ITempDataDictionary" yazar), tempdata içerisine bilgi atarken bir key bilgisine ihtiyacımız var ve value bilgisine ihtiyacımız var yani T bilgisi yani hangi tip bilgiyi atmak istiyoruz örneğin bizim projede AlertMessage. AlertMessage burada tip tanımlamamız olacak. Where ile de gelecek olan T'nin sınırlamasını yapabiliriz. Sadece class gelicek.
        public static void Put<T>(this ITempDataDictionary tempData, string key, T value) where T : class
        {
            tempData[key] = JsonConvert.SerializeObject(value);// tempData'nın içerisine key bilgisini tanımlarız (yani TempData["message" deki message kısmı], value değerinide serilize ediyoruz
        }
        // Get: serilize ettiğimiz tempdata içerisindeki bilgiyi deserilize ederek geri almamız gerekiyor. Bu bilgiyi alırken key bilgisine göre alırız. tempData üzerinden verdiğimiz key bilgisi ile bu bilgi bize gelicek. Geri dönüş değeride T olacak.
        public static T Get<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            object o;// Geri alacağımız bilgiyi ilk başta bir obje içerisine alırız. Daha sonra aşağıda olduğu gibi deserilize ederiz. Şuanda tempdata genel bir tip içerisinde. ITempDataDictionary türündeki tempData üzerinden TryGetValue ile key bilgisini veriyoruz yani message'yi ve bu bilgiyide  "out o" diyerek dışarıda ki bilginin(object o) içerisine atamayı yaparız. return ile geriye döndürürken kontrol işlemi yaparız. Eğer "o" bilgisi null ile geri null değer döndürür T o zaman null olacak, null değil ise bunu T'ye deserilize ederiz. deserilize etmeden önce json objesini(yani "o") string bilgiye çeviririz "(string)o)" şeklinde. 

            tempData.TryGetValue(key, out o);
            return o == null ? null : JsonConvert.DeserializeObject<T>((string)o);
        }

        // Bu extension class'ının namespace'ini _Viewlmports'a eklemeliyiz. Yoksa bu bilgiyi göremeyiz.
    }
}
