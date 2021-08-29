using InstitutionAPI.Config;
using InstitutionAPI.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using ResumeInfastructure.CosmosDbData.Extension;

namespace InstitutionAPI {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            services.SetupCosmosDb(Configuration);

            services.AddAutoMapper(typeof(Startup));

            services.AddControllers().AddNewtonsoftJson();
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "InstitutionAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if ( env.IsDevelopment() ) {
                app.UseDeveloperExceptionPage();
                app.EnsureCosmosDbIsCreated();
                app.SeedInstitutionContainerIfEmptyAsync().Wait();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "InstitutionAPI v1"));
            }

            app.UseCors(policy =>
                policy.AllowAnyOrigin()//.WithOrigins("http://localhost:5000", "https://localhost:5001")
                      .AllowAnyMethod()
                      .WithHeaders(HeaderNames.ContentType));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
