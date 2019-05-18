using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OData.Edm;
using NetCore.Data;
using Swashbuckle.AspNetCore.Swagger;

namespace NetCore.Web.UsersApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (string.IsNullOrEmpty(Configuration.GetConnectionString("AzureRedisConnection")))
            {
                throw new Exception("Invalid Configuration");
            }

            services.AddDbContext<AuctionsStoreContext>(option => option.UseInMemoryDatabase("AuctionsContext"));
            services.AddOData();
            services.AddODataQueryFilter();

            services.AddMvc(options => options.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDistributedRedisCache(
                option =>
                {
                    option.Configuration = Configuration.GetConnectionString("AzureRedisConnection");
                    option.InstanceName = "master";
                });

            services.AddSwaggerGen(
                c => { c.SwaggerDoc("v1", new Info {Title = "Auctions Api", Version = "v1"}); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc(builder =>
            {
                builder.Count().Filter().OrderBy().Expand().Select().MaxTop(null);
                builder.EnableDependencyInjection();

                builder.MapODataServiceRoute("odata", "odata", GetEdmModel());
            });

            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auctions Api"); });
        }

        private static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            var auctionsSet = builder.EntitySet<Auction>("Auctions");
            var usersSet = builder.EntitySet<User>("Users");
            builder.ComplexType<Vehicle>();
            builder.ComplexType<Engine>();
            //usersSet.EntityType.ContainsMany(item => item.Auctions);
            //usersSet.EntityType.ContainsRequired(item => item.Address);
            //usersSet.HasManyBinding(item => item.Auctions, auctionsSet);
            //builder.EntityType<Auction>();
            return builder.GetEdmModel();
        }
    }
}
