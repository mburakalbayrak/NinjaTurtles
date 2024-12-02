﻿using Autofac;
using AutoMapper;
using NinjaTurtles.Business.Abstract;
using NinjaTurtles.Business.Concrete;
using NinjaTurtles.Core.Utilities.Security.Jwt;
using NinjaTurtles.DataAccess.Abstract;
using NinjaTurtles.DataAccess.Concrete.EntityFramework;

namespace NinjaTurtles.Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AutoMapperProfiles>().As<Profile>();

            builder.RegisterType<EfProductDal>().As<IProductDal>();
            builder.RegisterType<EfCustomerDal>().As<ICustomerDal>();
            builder.RegisterType<EfCustomerQrVerificationDal>().As<ICustomerQrVerificationDal>();
            builder.RegisterType<EfParamItemDal>().As<IParamItemDal>();
            builder.RegisterType<EfCompanyOrderDal>().As<ICompanyOrderDal>();
            builder.RegisterType<EfCompanyOrderDetailDal>().As<ICompanyOrderDetailDal>();
            builder.RegisterType<EfQrCodeMainDal>().As<IQrCodeMainDal>();
            builder.RegisterType<EfQrCodeHumanDetailDal>().As<IQrCodeHumanDetailDal>();
            builder.RegisterType<EfQrCodeAnimalDetailDal>().As<IQrCodeAnimalDetailDal>();
            builder.RegisterType<EfQrLogDal>().As<IQrLogDal>();
            builder.RegisterType<EfUserDal>().As<IUserDal>();

            builder.RegisterType<QrManager>().As<IQrService>();
            builder.RegisterType<CustomerManager>().As<ICustomerService>();
            builder.RegisterType<CompanyOrderManager>().As<ICompanyOrderService>();
            builder.RegisterType<ProductManager>().As<IProductService>();
            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<AuthManager>().As<IAuthService>();
            builder.RegisterType<JwtHelper>().As<ITokenHelper>();

            builder.Register(ctx => new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
            }).CreateMapper()).As<IMapper>().InstancePerLifetimeScope();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            //builder.RegisterAssemblyTypes(assembly)
            //         .Where(t => t.Name.EndsWith("Service")) // İsimleri "Service" ile biten sınıfları bul
            //          .AsImplementedInterfaces()             // Bu sınıfları kendi interface'lerine register et
            //             .InstancePerLifetimeScope();
        }
    }
}
