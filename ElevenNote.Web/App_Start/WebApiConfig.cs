using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

// get rid of .App_Start on this name
namespace ElevenNote.Web
{
    public static class WebApiConfig
    {
        public static void Register()
        {
            GlobalConfiguration
               .Configure(
                   x =>
                   {
                       x
                           .Formatters
                           .JsonFormatter
                           .SupportedMediaTypes
                           .Add(new MediaTypeHeaderValue("text/html"));

                       x.MapHttpAttributeRoutes();
                   }
               );
        }
    }
}