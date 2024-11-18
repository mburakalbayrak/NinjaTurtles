using Autofac;
using AutoMapper;
using NinjaTurtles.DataAccess.Abstract;
using NinjaTurtles.DataAccess.Concrete.EntityFramework;

namespace NinjaTurtles.Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EfProductDal>().As<IProductDal>();
            builder.RegisterType<AutoMapperProfiles>().As<Profile>();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            //builder.RegisterAssemblyTypes(assembly)
            //         .Where(t => t.Name.EndsWith("Service")) // İsimleri "Service" ile biten sınıfları bul
            //          .AsImplementedInterfaces()             // Bu sınıfları kendi interface'lerine register et
            //             .InstancePerLifetimeScope();
        }
    }
}
