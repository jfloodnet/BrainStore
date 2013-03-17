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
        public static IContainer Configure(HttpConfiguration cfg)
        {
            var builder = new ContainerBuilder();

            builder.Register(ctx => EventStoreConnectionFactory.Create())
                   .SingleInstance();
            builder.RegisterType<EventStoreReader>()
                   .AsSelf().InstancePerLifetimeScope();

            builder.RegisterType<RelayService>()
                   .WithParameter(
                    new TypedParameter(typeof (Func<Uri, Uri>), 
                    new Func<Uri, Uri>(uri => RelayUri(uri))))
                    .InstancePerLifetimeScope();

            builder.RegisterType<LinkInterceptor>().AsSelf()
                   .WithParameter(new NamedParameter("host", Config.Host))
                   .WithParameter(new NamedParameter("port", Config.Port))
                   .WithParameter(new TypedParameter(typeof (HttpConfiguration), cfg))
                   .InstancePerLifetimeScope();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<LiveEmotivSessionHub>().InstancePerLifetimeScope();
            builder.RegisterType<HistoricalEmotivSessionHub>().InstancePerLifetimeScope();

            builder.RegisterType<LiveEmotivSessionSubscriber>()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<HistoricalEmotivSessionReader>()
                   .AsSelf()
                   .InstancePerLifetimeScope();

            return builder.Build();
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