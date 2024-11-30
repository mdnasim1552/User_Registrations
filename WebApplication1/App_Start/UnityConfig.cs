using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Mvc5;
using WebApplication1.Data;
using WebApplication1.IRepositories;
using WebApplication1.Repositories;

namespace WebApplication1
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers
            // Register IDbConnection
            container.RegisterFactory<IDbConnection>(_ =>
            {
                var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                return new SqlConnection(connectionString);
            });
            container.RegisterType<IProcessAccess, ProcessAccess>();
            container.RegisterType<IUserRepository, UserRepository>();
            // e.g. container.RegisterType<ITestService, TestService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}