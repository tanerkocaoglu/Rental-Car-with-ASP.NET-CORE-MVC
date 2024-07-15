# Rental Car with ASP.NET Core MVC

Bu proje, ASP.NET Core MVC kullanarak geliştirilmiş bir araç kiralama sistemidir. Kullanıcılar, mevcut araçları görüntüleyebilir, kiralayabilir ve kiralama geçmişlerini takip edebilirler.

## Özellikler

- Kullanıcı kaydı ve oturum açma
- Araç listeleme ve detay görüntüleme
- Araç kiralama işlemleri
- Kiralama geçmişi takibi
- Yönetici paneli ile araç ekleme, güncelleme ve silme

## Gereksinimler

Bu projeyi çalıştırmak için aşağıdaki yazılımlara ihtiyacınız olacak:

- .NET 6.0 veya daha yeni bir sürüm
- Visual Studio 2022 veya daha yeni bir sürüm
- SQL Server (veya başka bir veritabanı sağlayıcısı)

## Kurulum

Bu projeyi kendi bilgisayarınızda çalıştırmak için şu adımları izleyebilirsiniz:

 1. Bu projeyi klonlayın:
   ```sh
   git clone https://github.com/tanerkocaoglu/Rental-Car-with-ASP.NET-CORE-MVC.git
  ```
2. Visual Studio'da projeyi açın:  
  ```sh
   cd Rental-Car-with-ASP.NET-CORE-MVC
   start RentalCarSln.sln
   ```
3. Veritabanı bağlantı dizesini güncelleyin (appsettings.json dosyasında).

4. Projeyi derleyin ve çalıştırın:
  ```sh
  F5 tuşuna basarak ya da Debug menüsünden Start Debugging seçeneğini tıklayarak projeyi çalıştırabilirsiniz.
```

## Kullanım
Kullanıcılar, sisteme kaydolduktan sonra oturum açarak araçları görüntüleyebilir ve kiralayabilir. 
Yönetici olarak giriş yaparak araçları ekleyebilir, güncelleyebilir veya silebilirsiniz.
