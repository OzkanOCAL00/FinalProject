using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.ValidationRules.FluentValidation
{
    public class ProductValidator:AbstractValidator<Product>//AbstractValidator NuGet FluentValidation dan using ediliyor
    {
        //kuralları ctor ların içine yazıyoruz
        public ProductValidator()
        {//RuleFor kim için kural demek
            RuleFor(p => p.ProductName).NotEmpty();//boş olamaz
            RuleFor(p =>p.ProductName).MinimumLength(2);//en az iki karakter olmalı
            RuleFor(p => p.UnitPrice).NotEmpty();
            RuleFor(p => p.UnitPrice).GreaterThan(0);//0 dan büyük olmalı
            RuleFor(p => p.UnitPrice).GreaterThanOrEqualTo(10).When(p => p.CategoryId == 1);//10 dan büyük olmalı ne zaman p nin CategoryId si 1 e eşit olduğu zaman
            RuleFor(p => p.ProductName).Must(StartWithA).WithMessage("Ürünler A harfi ile başlamalı");//burada olmayan bi komuta yazıyoruz komutumuz A ile başlamalı   method ekliyoruz burada ampulden generate method diyerek ekliyoruz    
        }//Ctrl K+D kodları düzenler //WithMessage() ek bi mesaj vermek istersek kullanabiliriz

        private bool StartWithA(string arg)//bool = eğer true döndürürsen kurala uygun false döndürürsen uygun değil demek
        {//arg ise gönderdiğimiz parametre yani ProductName
            return arg.StartsWith("A");
        }
    }
}
