using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using IEventSourcedMyBrain.App_Start;
using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.AspNet.SignalR;


namespace IEventSourcedMyBrain
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            var container = ApplicationConfigurator.Configure(GlobalConfiguration.Configuration);

            var autofacSignalRAdaptor = new DepedencyResolverWrapper(new Autofac.Integration.SignalR.AutofacDependencyResolver(container));
            GlobalHost.DependencyResolver = autofacSignalRAdaptor;
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);


            RouteTable.Routes.MapHubs();

            BundleConfig.RegisterBundles(BundleTable.Bundles);
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_Error(Object sender, EventArgs e)
        {

        }
    }
}