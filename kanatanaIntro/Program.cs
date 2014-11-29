using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin.Hosting;
using Owin;

// Install-Package -IncludePrerelease Microsoft.owin.hosting
// Install-Package -IncludePrerelease Microsoft.owin.host.httplistener
// install-package -includePrerelease microsoft.owin.diagnostics
// install-package =includeprerelease microsoft.aspnet.webapi.owinselfhost
// install-package -includeprerelease microsoft.owin.host.systemweb
namespace kanatanaIntro
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    /*class Program - not needed for IIS
    {
        static void Main(string[] args)
        {
            string uri = "http://localhost:8080";
            using (WebApp.Start<Startup>(uri))
            {
                Console.WriteLine("Started");
                Console.ReadKey();
                Console.WriteLine("Ending");

            }
        }
    }
*/
    public static class AppBuilderExtensions
    {
        public static void UseHelloWorld(this IAppBuilder app)
        {
            app.Use<HelloWorldComponent>();        
        }
    }

    public class HelloWorldComponent
    {
        private readonly AppFunc _next;
        public HelloWorldComponent(AppFunc next)
        {
            _next = next;
        }

        public Task Invoke(IDictionary<string, object> environment)
        {
            var response = environment["owin.ResponseBody"] as Stream;
            using (var writer = new StreamWriter(response))
            {
                return writer.WriteAsync("Hello!");
            }
        }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // app.UseWelcomePage();
            //app.UseHelloWorld();

            /*app.Use(async (environment, next) =>
            {
                foreach (var pair in environment.Environment)
                {
                    Console.WriteLine("key: {0}, Value {1}", pair.Key, pair.Value);
                }
                Console.WriteLine();
                await next();
            });*/

            app.Use(async (environment, next) =>
            {
                Console.WriteLine("Request Path: {0}", environment.Request.Path);
                
                await next();
                Console.WriteLine("Response Status: {0}", environment.Response.StatusCode);
            });

            ConfigureWebApi(app);
            app.Use<HelloWorldComponent>();

            /*app.Run( context =>
             {
                 return context.Response.WriteAsync("Hello from the app!");
             });*/
        }

        private void ConfigureWebApi(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                "DefaultApi", 
                "api/{controller}/{id}", 
                new {id = RouteParameter.Optional});
            app.UseWebApi(config);
        }
    }
}
