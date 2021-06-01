using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule:Module//Autofac modül olduğunu söyledik
    {
        protected override void Load(ContainerBuilder builder)//over deyip space ye bastık load ı bulduk yapı oluştu
        {
            builder.RegisterType<ProductManager>().As<IProductService>().SingleInstance();//biri IProductService isterse ona ProductManager i ver demek
            builder.RegisterType<EfProductDal>().As<IProductDal>().SingleInstance();//EfProductDal ı new le ver demek Sürekli new lenmesi yerine SingleInstance() bir defa new leyip herkese veriyor onu

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();
        }
    }
}
