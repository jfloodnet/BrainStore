using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace IEventSourcedMyBrain
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var relaycontroller = new 
            { 
                controller = "StreamRelay", 
                action = "Get",
                match = RouteParameter.Optional,
                any = RouteParameter.Optional,
                combination = RouteParameter.Optional,
                of = RouteParameter.Optional,
                url = RouteParameter.Optional,
                parts = RouteParameter.Optional
            };

            config.Routes.MapHttpRoute(
                name: "Streams",
                routeTemplate: "streams/{match}/{any}/{combination}/{of}/{url}/{parts}",
                defaults: relaycontroller
            );

            config.Routes.MapHttpRoute(
                name: "EventStoreApi",
                routeTemplate: "es/{controller}/{id}/",
                defaults: new { id = RouteParameter.Optional }
            ); 
        }
    }
}
