using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Edm;
using Microsoft.AspNet.OData.Builder;
using static EJ2APIServices.Controllers.OrdersController;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Routing;
using Microsoft.OData.UriParser;
using Microsoft.AspNetCore.Cors.Infrastructure;
using EJ2APIServices.Models;
using EJ2APIServices.Controllers;


namespace EJ2APIServices
{
    public class Startup
    {
        public IConfiguration ConfigConnectionString;
        private string _contentRootPath = "";
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            ConfigConnectionString = configuration;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            _contentRootPath = env.ContentRootPath;
            ServerPath.MapPath = _contentRootPath;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOData();
            services.AddMvc()
.AddJsonOptions(x =>
{
    x.SerializerSettings.ContractResolver = null;
});

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });
            string connection = Configuration.GetConnectionString("Test");
            if (connection.Contains("%CONTENTROOTPATH%"))
            {
                connection = connection.Replace("%CONTENTROOTPATH%", _contentRootPath);

            }
            services.AddEntityFrameworkNpgsql().AddDbContext<EEJ2SERVICEEJ2WEBSERVICESSRCAPP_DATADIAGRAMMDFContext>(options => options.UseNpgsql(connection));
            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("AllowAllOrigins");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(b =>
            {
                b.Count().Filter().OrderBy().Expand().Select().MaxTop(null);
                b.MapODataServiceRoute("ODataRoute", "odata", GetEdmModel());
            });

        }
        private static Microsoft.OData.Edm.IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            var order = builder.EntitySet<OrdersDetails>("Orders");
            var emp = builder.EntitySet<Employees>("Employees");
            var tree = builder.EntitySet<SelfReferenceData>("SelfReferenceData");
            return builder.GetEdmModel();
        }

    }

    public class ServerPath
    {
        public static string MapPath = "";
    }
}
