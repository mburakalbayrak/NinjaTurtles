using Autofac;
using AutoMapper;
using NinjaTurtles.DataAccess.Abstract;
using NinjaTurtles.DataAccess.Concrete.EntityFramework;

namespace NinjaTurtles.Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EfProductDal>().As<IProductDal>();
            builder.RegisterType<AutoMapperProfiles>().As<Profile>();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();


        }
    }
}
