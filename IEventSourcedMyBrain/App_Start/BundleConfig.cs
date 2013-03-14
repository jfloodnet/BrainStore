using System;
using System.Collections.Generic;
using System.IO;
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

            AddFileSetOrdering(bundles);

            bundles.Add(new ScriptBundle("~/Scripts/eventstore").IncludeDirectory(
                    "~/web/es/projections/", "*.js")
            );

            bundles.Add(new ScriptBundle("~/Scripts/charts").IncludeDirectory(
                "~/web/es/lib/charts/js/", "*.js").Include(
                  "~/web/es/js/es.chartSetup.js")
            );

            bundles.Add(new ScriptBundle("~/Scripts/controls").IncludeDirectory(
                "~/web/es/js/controls/", "es.*")
            );


            bundles.Add(new ScriptBundle("~/Scripts/bootstrap").Include(
                "~/web/es/lib/bootstrap/js/bootstrap.js")
            );

            bundles.Add(new StyleBundle("~/Styles/charts").IncludeDirectory(
                "~/web/es/lib/charts/css/", "*.css").Include(
                "~/web/es/css/es.charts.css")
            );

            bundles.Add(new StyleBundle("~/Styles/layout").Include(
                "~/web/es/css/es.sitelayout.css",
                "~/web/es/lib/jquery/jquery-ui-1.8.23.custom.css")
            );

            bundles.Add(new StyleBundle("~/Styles/bootstrap").IncludeDirectory(
                "~/web/es/lib/bootstrap/css/", "*.css")
            );
        }

        private static void AddFileSetOrdering(BundleCollection bundles)
        {
            var fileSet = new BundleFileSetOrdering("projections");
            fileSet.Files.Add("Projections.js");
            bundles.FileSetOrderList.Add(fileSet);
        }
    }
}