using Core.Entities.Concrete;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Business.Constants
{//Constants proje sabitlerini yazacağımız yerdir
    //static eklendi
    public static class Messages
    {
        public static string ProductAdded = "Ürün eklendi";
        public static string ProductNameInvalid = "Ürün ismi geçersiz";
        public static string MeintenanceTime = "Sistem bakımda";
        public static string ProductsListed = "Ürünler listelendi";
        public static string Errors = "Hata!";
        public static string ProductCountOfCategoryError="Kategoriye en fazla 10 ürün ekleyebilirsiniz";
        public static string ProductNameAlreadyExists = "Bu isimde zaten başka bir ürün var";
        public static string CategoryLimitExceded = "Kategori limiti aşıldıüı için yeni ürün eklenemiyor";
        public static string AuthorizationDenied= "Yetkiniz yok.";
        public static string UserRegistered= "Kayıt oldu";
        public static string UserNotFound = "Kullanıcı bulunamadı";
        public static string PasswordError = "Parola hatası";
        public static string SuccessfulLogin= "Başarılı giriş";
        public static string UserAlreadyExists= "Kullanıcı mevcut";
        public static string AccessTokenCreated= "Token oluşturuldu";
    }
}
