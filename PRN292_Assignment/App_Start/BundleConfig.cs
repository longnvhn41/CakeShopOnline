using System.Web;
using System.Web.Optimization;

namespace CakeShop
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/jquery", "https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jquery", "https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jquery", "https://code.jquery.com/jquery-3.3.1.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap", "https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap", "https://kit.fontawesome.com/ec811f6ffe.js"));
            //bundles.Add(new StyleBundle("~/Content/css", "https://fonts.googleapis.com/icon?family=Material+Icons").Include("~/Content/bootstrap.css"));
            bundles.Add(new StyleBundle("~/Content/css", "https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js").Include("~/Content/bootstrap.css"));
            //bundles.Add(new StyleBundle("~/Content/css", "https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/css/bootstrap.min.css").Include("~/Content/bootstrap.css"));
            bundles.Add(new StyleBundle("~/Content/css", "https://use.fontawesome.com/releases/v5.3.1/css/all.css").Include("~/Content/bootstrap.css"));
            bundles.Add(new StyleBundle("~/Content/css", "https://fonts.googleapis.com/css?family=Roboto|Varela+Round").Include("~/Content/bootstrap.css"));
            bundles.Add(new StyleBundle("~/Content/css", "https://fonts.googleapis.com/icon?family=Material+Icons").Include("~/Content/bootstrap.css"));
            bundles.Add(new StyleBundle("~/Content/css", "https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css").Include("~/Content/bootstrap.css"));
            bundles.Add(new StyleBundle("~/Content/css", "https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css").Include("~/Content/bootstrap.css"));
            bundles.Add(new StyleBundle("~/Content/css", "https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css").Include("~/Content/bootstrap.css"));

            BundleTable.EnableOptimizations = true;
            bundles.UseCdn = true;
        }
    }
}