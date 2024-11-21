using Autofac;
using AutoMapper;
using NinjaTurtles.Business.Abstract;
using NinjaTurtles.Business.Concrete;
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
            builder.RegisterType<EfQrCodeDetailDal>().As<IQrCodeDetailDal>();
            builder.RegisterType<EfQrLogDal>().As<IQrLogDal>();
            builder.RegisterType<EfUserDal>().As<IUserDal>();

            builder.RegisterType<CustomerManager>().As<ICustomerService>();
            builder.RegisterType<CompanyOrderManager>().As<ICompanyOrderService>();
            builder.RegisterType<ProductManager>().As<IProductService>();
            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<AuthManager>().As<IAuthService>();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            //builder.RegisterAssemblyTypes(assembly)
            //         .Where(t => t.Name.EndsWith("Service")) // İsimleri "Service" ile biten sınıfları bul
            //          .AsImplementedInterfaces()             // Bu sınıfları kendi interface'lerine register et
            //             .InstancePerLifetimeScope();
        }
    }
}
