using ExternalServices;
using GraphiQl;
using GraphQL;
using GraphQL.Types;
using graphql_with_external_services.Mutations;
using graphql_with_external_services.Queries;
using graphql_with_external_services.Schema;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Types;

namespace graphql_with_external_services
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddMvc()
                .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddHttpClient<IMicroregiaoExternalService, MicroregiaoExternalService>();
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddSingleton<MicroregiaoQuery>();
            services.AddSingleton<MicroregiaoType>();
            services.AddSingleton<MesorregiaoType>();
            services.AddSingleton<UFType>();
            services.AddSingleton<RegiaoType>();
            services.AddSingleton<PropertyMutation>();
            services.AddSingleton<MicroregiaoInputType>();

            var sp = services.BuildServiceProvider();

            services.AddSingleton<ISchema>(new RealEstateSchema(new FuncDependencyResolver(type => sp.GetService(type))));

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader();
                });
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseGraphiQl();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
