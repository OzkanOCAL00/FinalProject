using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    //SOLID (I) harfi kullanmayacağın birşeyi yazma demek
    public interface IProductService
    {//IDataResult hem mesajı hem de data da ki yapıyı(List<Product> döndüre bilecek birşey olacak
        //List<Product> artık T oldu
        IDataResult<List<Product>> GetAll();
        IDataResult<List<Product>> GetAllByCategoryId(int id);
        IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max);
        IDataResult<List<ProductDetailDto>> GetProductDetails();
        IDataResult<Product> GetById(int ProductId);//sadece ürün ile ilgili bilgiler için yazılır
        IResult Add(Product product);//burda  yok o yüzden IDataResult olmaz
        IResult Update(Product product);


        IResult AddTransactionalTest(Product product);

    }
}
