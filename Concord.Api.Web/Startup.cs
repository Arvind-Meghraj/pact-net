﻿using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Concord.Api.Web.Controllers;
using Microsoft.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;

[assembly: OwinStartup("ApiConfiguration", typeof(Concord.Api.Web.Startup))]
namespace Concord.Api.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();

            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            json.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            json.SerializerSettings.Formatting = Formatting.None;
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            var builder = new ContainerBuilder();
            
            builder.RegisterApiControllers(typeof(TestController).Assembly);
            var container = builder.Build();

            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);
        }
    }
}