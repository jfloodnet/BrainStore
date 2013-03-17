using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using IEventSourcedMyBrain.Controllers;
using IEventSourcedMyBrain.Hubs;
using Microsoft.AspNet.SignalR;

namespace IEventSourcedMyBrain
{
    public static class ApplicationConfigurator
    {
        public static void Configure(HttpConfiguration cfg)
        {
            var builder = new ContainerBuilder();

            builder.Register(ctx => EventStoreConnectionFactory.Create())
                   .SingleInstance();
            builder.RegisterType<EventStoreReader>()
                   .AsSelf().SingleInstance();

            builder.RegisterType<RelayService>()
                   .WithParameter(
                    new TypedParameter(typeof (Func<Uri, Uri>), 
                    new Func<Uri, Uri>(uri => RelayUri(uri))))
                    .InstancePerHttpRequest();

            builder.RegisterType<LinkInterceptor>().AsSelf()
                   .WithParameter(new NamedParameter("host", Config.Host))
                   .WithParameter(new NamedParameter("port", Config.Port))
                   .WithParameter(new TypedParameter(typeof (HttpConfiguration), cfg))
                   .InstancePerHttpRequest();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<LiveEmotivSessionSubscriber>()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<HistoricalEmotivSessionReader>()
                .AsSelf()
                .SingleInstance(); 
            
            var container = builder.Build();

            Task.Run(() => container.Resolve<LiveEmotivSessionSubscriber>().Subscribe());

            GlobalHost.DependencyResolver = new DepedencyResolverWrapper(new Autofac.Integration.SignalR.AutofacDependencyResolver(container));
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            cfg.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }


        private static Uri RelayUri(Uri requestUri)
        {
            var ub = new UriBuilder(requestUri);
            ub.Host = Config.Host;
            ub.Port = Config.Port;
            ub.Query = "format=json";
            return ub.Uri;
        }
    }

    public class DepedencyResolverWrapper : Microsoft.AspNet.SignalR.IDependencyResolver
    {
        private readonly Microsoft.AspNet.SignalR.IDependencyResolver inner;
        public DepedencyResolverWrapper(Microsoft.AspNet.SignalR.IDependencyResolver inner)
        {
            this.inner = inner;
        }
        public object GetService(Type serviceType)
        {
            return inner.GetService(serviceType);
        }

        public System.Collections.Generic.IEnumerable<object> GetServices(Type serviceType)
        {
            return this.inner.GetServices(serviceType);
        }

        public void Register(Type serviceType, System.Collections.Generic.IEnumerable<Func<object>> activators)
        {
            inner.Register(serviceType, activators);
        }

        public void Register(Type serviceType, Func<object> activator)
        {
            inner.Register(serviceType, activator);
        }

        public void Dispose()
        {
            this.inner.Dispose();
        }
    }

}