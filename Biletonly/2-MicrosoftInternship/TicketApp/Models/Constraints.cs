using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketApp.Models
{
    public static class Constraints
    {
        public const double StationsMinimumUpdateHour = 96;
        public const string AppName = "Biletonly";

        public const string Laws =
            "1. Biletonly, bilet işlemlerini üçüncü parti şirket ve servisler kullanarak gerçekleştirir. Oluşabilecek sorunlardan Biletonly sorumlu tutulamaz.\r\n" +
            "2. Sefer sırasında gerçekleşebilecek aksaklık, sorun ve kazalardan Biletonly sorumlu değildir. Bu gibi durumlar taşıyıcı firma sorumluluğundadır.\r\n" +
            "3. Uygulamayı kullanan kullanıcıların girdiği veriler kullanıcıların sorumluluğundadır.\r\n" +
            "4. Taşıyıcı firma sefer detaylarında (saat, tarih vs.) değişiklik yapma hakkına sahiptir.\r\n" +
            "5. Bilet işlemlerini gerçekleştiren kullanıcılar, taşıyıcı firmaların uyguladığı koşullar ve kısıtlamaları kabul etmiş sayılırlar.\r\n" +
            "6. Gününde ve saatinde kullanılmayan biletler geçersiz sayılmakta olup, iptali Biletonly tarafından yapılamaz.\r\n" +
            "7. Satın alınan biletlerin iptali iade kuralları çerçevesinde yapılabilir.\r\n" +
            "8. Satın alınan biletlerin iptal kuralları firmaların belirledikleri kurallar çerçevesinde değişebilmektedir. Biletonly, biletlerin iptali konusunda sarumlu tutulamaz.\r\n" + 
            "9. Biletonly uygulamasını kullanan ve bilet işlemleri gerçekleştiren kullanıcılar bu sözleşmeyi peşinen kabul etmiş sayılırlar.";

        public const string ApiUsername = "OK.TicketApp.WindowsPhone";
        public const string ApiPassword = "API_PASS";
        public const string ApiVersion = "1.0";
    }
}
