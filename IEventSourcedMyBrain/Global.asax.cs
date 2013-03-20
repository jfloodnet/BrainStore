using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using IEventSourcedMyBrain.App_Start;
using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.AspNet.SignalR;
using SignalRResolver = Autofac.Integration.SignalR.AutofacDependencyResolver;

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

            WireUpResolvers(container);            

            RouteTable.Routes.MapHubs();

            BundleConfig.RegisterBundles(BundleTable.Bundles);
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        private void WireUpResolvers(IContainer container)
        {
            //SignalR
            GlobalHost.DependencyResolver = new DepedencyResolverWrapper(new SignalRResolver(container)); 
            //MVC
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            //WebApi
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        protected void Application_Error(Object sender, EventArgs e)
        {

        }
    }
}