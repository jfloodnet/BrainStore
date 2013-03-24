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
using Autofac.Builder;
using System.Collections.Generic;
using System.Linq;

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

            builder.RegisterType<LiveEmotivSessionHub>();
            builder.RegisterType<HistoricalEmotivSessionHub>();

            builder.RegisterType<LiveEmotivSessionSubscriber>()
                .AsSelf()
                .InstancePerLifetimeScope();

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
}

