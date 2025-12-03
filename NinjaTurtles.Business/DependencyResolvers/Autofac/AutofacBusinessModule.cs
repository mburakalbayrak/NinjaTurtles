using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Http;
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
            // Burayı bilerek yorum satırına aldık. Canlı api 500 alıyordu bundan kaynaklı olabilir
            //builder.RegisterType<AutoMapperProfiles>().As<Profile>();   

            builder.RegisterType<EfProductDal>().As<IProductDal>().InstancePerLifetimeScope();
            builder.RegisterType<EfCustomerDal>().As<ICustomerDal>().InstancePerLifetimeScope();
            builder.RegisterType<EfCustomerQrVerificationDal>().As<ICustomerQrVerificationDal>().InstancePerLifetimeScope();
            builder.RegisterType<EfParamItemDal>().As<IParamItemDal>().InstancePerLifetimeScope();
            builder.RegisterType<EfCompanyDal>().As<ICompanyDal>().InstancePerLifetimeScope();
            builder.RegisterType<EfCompanyOrderDetailDal>().As<ICompanyOrderDetailDal>().InstancePerLifetimeScope();
            builder.RegisterType<EfQrCodeMainDal>().As<IQrCodeMainDal>().InstancePerLifetimeScope();
            builder.RegisterType<EfQrCodeHumanDetailDal>().As<IQrCodeHumanDetailDal>().InstancePerLifetimeScope();
            builder.RegisterType<EfQrCodeAnimalDetailDal>().As<IQrCodeAnimalDetailDal>().InstancePerLifetimeScope();
            builder.RegisterType<EfQrLogDal>().As<IQrLogDal>().InstancePerLifetimeScope();
            builder.RegisterType<EfUserDal>().As<IUserDal>().InstancePerLifetimeScope();
            builder.RegisterType<EfCustomerContractDal>().As<ICustomerContractDal>().InstancePerLifetimeScope();
            builder.RegisterType<EfSupportTaskDal>().As<ISupportTaskDal>().InstancePerLifetimeScope();

            builder.RegisterType<QrManager>().As<IQrService>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerManager>().As<ICustomerService>().InstancePerLifetimeScope();
            builder.RegisterType<CompanyManager>().As<ICompanyService>().InstancePerLifetimeScope();
            builder.RegisterType<ProductManager>().As<IProductService>().InstancePerLifetimeScope();
            builder.RegisterType<ParamManager>().As<IParamService>().InstancePerLifetimeScope();
            builder.RegisterType<UserManager>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<AuthManager>().As<IAuthService>().InstancePerLifetimeScope();
            builder.RegisterType<SupportTaskManager>().As<ISupportTaskService>().InstancePerLifetimeScope();
            builder.RegisterType<JwtHelper>().As<ITokenHelper>().InstancePerLifetimeScope();

            builder.RegisterType<HttpContextAccessor>()
         .As<IHttpContextAccessor>()
         .SingleInstance();
            builder.Register(ctx => new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
            }).CreateMapper()).As<IMapper>().SingleInstance();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            //builder.RegisterAssemblyTypes(assembly)
            //         .Where(t => t.Name.EndsWith("Service")) // İsimleri "Service" ile biten sınıfları bul
            //          .AsImplementedInterfaces()             // Bu sınıfları kendi interface'lerine register et
            //             .InstancePerLifetimeScope();
        }
    }
}
