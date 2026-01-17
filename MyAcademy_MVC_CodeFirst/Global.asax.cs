using AutoMapper;
using MyAcademy_MVC_CodeFirst.Mappings;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MyAcademy_MVC_CodeFirst
{
    public class MvcApplication : System.Web.HttpApplication
    {

        public static IMapper mapperInstance;
        protected void Application_Start()
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(FeatureMappings));
            });

            //config.AssertConfigurationIsValid();// check if mappings are suitable and give an error if it is not

            mapperInstance = config.CreateMapper();

            AreaRegistration.RegisterAllAreas();
            GlobalFilters.Filters.Add(new
                AuthorizeAttribute());
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
