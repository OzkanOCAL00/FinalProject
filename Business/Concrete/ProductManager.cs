using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.CCS;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Business.Concrete
{//Bir Entity Manager kendisi hariç başka Dal ı enjekte edemez etmez.. Onun yerine örneğin CategoryId yi kullanacaksak iş komutu olarak ICategoryService yi injection ederiz
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
        ICategoryService _categoryService;

        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;
        }

        //Claim(İddia etmek) yani yetkisi var anlamında
        [SecuredOperation("product.add,admin")]
        [ValidationAspect(typeof(ProductValidator))]//aşagıdaki metodu doğrula ProductValidator ü kullanarak demek(Bu bir Attribute)
        [CacheRemoveAspect("IProductService.Add")]
        public IResult Add(Product product)
        {
            //business codes
            //validation (Burada product ın yapısal uyumunu kontrol için yazılan kodlar doğrulama oluyor)ama örneğin bir kişi kredi başvurusu yapıyor ve o kişinin başvuru nitelikleri karşılanıyor mu diye kontrol edilmesi ve verilip verilmemesi validation değildir

            //resul kurala uymayan var ise doludur yok ise boştur hata kımını içine attık yani
           IResult result = BusinessRules.Run(CheckIfProductNameExists(product.ProductName),
                CheckIfProductCountOfCategoryCorrect(product.CategoryId), CheckIfCategoryLimitExceded());

            if(result != null)//burada result null değilse yani kurala uymayan bir durum var ise
            {
                return result;
            }

            _productDal.Add(product);

            return new SuccessResult(Messages.ProductAdded);

        }

        [CacheAspect]//key,value //Belli bir süre yapılan istekler bellekte server de tutuluyır data base ye gitmeye gerek kalmıyor
        public IDataResult<List<Product>> GetAll()
        {
            //iş kodları yazılıyor buraya
            if (DateTime.Now.Hour==1)
            {
                return new ErrorDataResult<List<Product>>(Messages.MeintenanceTime);//sadece mesaj döndürüyoruz
            }
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(),Messages.ProductsListed);//data ve işlem sonucunu döndürüyoruz
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p=> p.CategoryId==id));
        }

        [CacheAspect]
        [PerformanceAspect(5)]//5 saniye
        public IDataResult<Product> GetById(int ProductId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p=> p.ProductId == ProductId));
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p=> p.UnitPrice>=min && p.UnitPrice<=max));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails());
        }

        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Update(Product product)
        {
            var result = _productDal.GetAll(p => p.CategoryId == product.CategoryId).Count;
            if (result >= 10)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }
            throw new NotImplementedException();
        }

        //iş kuralı parçacıklarımızı private olarak yazıyoruz
        private IResult CheckIfProductCountOfCategoryCorrect(int categoryId)//kategorideki ürün sayısının kurallara uygunluğunu doğrula 
        {
            var result = _productDal.GetAll(p => p.CategoryId == categoryId).Count;//count sayıyı ver demek yani kaç tane ürün varsa
            if (result >= 15)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }
            return new SuccessResult();//şu kuraldan geçti diye kullanıcıya bilgi verilmediği için burada mesaj kullanmayız
        }
        private IResult CheckIfProductNameExists(string productName)//daha önce bu ürün eklenmiş mi eklenmemiş mi
        {
            var result = _productDal.GetAll(p => p.ProductName == productName).Any();//Any Linq ten geliyor aynısı var mı demek
            if(result)//zaten bu result ==true demek
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);//böyle bir ürün zaten var diye mesaj gönderiyoruz
            }
            return new SuccessResult();
        }

        private IResult CheckIfCategoryLimitExceded()
        {
            var result = _categoryService.GetAll();
            if (result.Data.Count>15)
            {
                return new ErrorResult(Messages.CategoryLimitExceded);
            }
            return new SuccessResult();
        }

        [TransactionScopeAspect]
        public IResult AddTransactionalTest(Product product)//aynı anda biri hesabından para gönderiyor diğerinin hesabına para yatmıyor hata veriyor o durumda bir önceki işleme dçnmek için bu kod ları kullanırız
        {
            
            Add(product);
            if (product.UnitPrice<10)
            {
                throw new Exception("");
            }
            Add(product);
            return null;
            
        }
    }
}
