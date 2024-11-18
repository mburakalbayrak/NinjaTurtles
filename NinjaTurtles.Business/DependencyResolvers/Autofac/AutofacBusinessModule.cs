using Autofac;
using NinjaTurtles.Business.Abstract;
using NinjaTurtles.Business.Concrete;
using NinjaTurtles.DataAccess.Abstract;
using NinjaTurtles.DataAccess.Concrete.EntityFramework;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTurtles.Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EfProductDal>().As<IProductDal>();


            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            //builder.RegisterAssemblyTypes(assembly)
            //         .Where(t => t.Name.EndsWith("Service")) // İsimleri "Service" ile biten sınıfları bul
            //          .AsImplementedInterfaces()             // Bu sınıfları kendi interface'lerine register et
            //             .InstancePerLifetimeScope();
        }
    }
}
