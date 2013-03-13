using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace IEventSourcedMyBrain.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/jquery").Include(
                "~/web/es/lib/jquery/jquery-{version}.js",
                "~/web/es/lib/jquery/jquery.*",
                "~/web/es/lib/jquery/jquery-ui-{version}.js")
            );

            bundles.Add(new ScriptBundle("~/Scripts/eventstore").Include(
                "~/web/es/projections/es.api.js",
                "~/web/es/projections/es.projections.environment.js",
                "~/web/es/projections/es.projections.js",
                "~/web/es/projections.js",
                "~/web/es/Prelude1.js",
                "~/web/es/projections/emoProjection.js")
            );

            bundles.Add(new ScriptBundle("~/Scripts/controls").Include(
               "~/web/es/js/controls/es.Sessions.js",
               "~/web/es/js/controls/es.TimeSeries.js",
               "~/web/es/js/controls/es.Zoomer.js",
               "~/web/es/js/controls/es.Selector.js")
            );

            bundles.Add(new ScriptBundle("~/Scripts/charts").Include(
                  "~/web/es/lib/charts/d3.min.js",
                  "~/web/es/lib/charts/d3.layout.min.js",
                  "~/web/es/lib/charts/rickshaw.js",
                  "~/web/es/js/es.chartSetup.js")
            );

            bundles.Add(new ScriptBundle("~/Scripts/bootstrap").Include(
                "~/web/es/lib/bootstrap/js/bootstrap.js")
            );

            bundles.Add(new StyleBundle("~/Styles/charts").Include(
                "~/web/es/lib/charts/d3.css",
                "~/web/es/lib/charts/rickshaw.min.css",
                "~/web/es/css/es.charts.css")
            );

            bundles.Add(new StyleBundle("~/Styles/layout").Include(
                "~/web/es/css/es.sitelayout.css")
            );

            bundles.Add(new StyleBundle("~/Styles/bootstrap").Include(
                "~/web/es/lib/bootstrap/css/bootstrap.css",
                "~/web/es/lib/bootstrap/css/bootstrap-responsive.css")
            );
        }
    }
}