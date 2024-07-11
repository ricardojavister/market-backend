using MarketApi.Models;
using MarketApi.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;

namespace MarketApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public readonly string _policyName = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(c =>
            {
                c.AddPolicy(_policyName,
                    options =>
                            Configuration.GetSection("CorsOrigins").Get<string[]>()
                                    .Aggregate(options, (op, origing) => op.WithOrigins(origing))
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials()//Add this line
                        );
            });

            services.Configure<MongoDBSettings>(
                Configuration.GetSection(nameof(MongoDBSettings)));

            services.AddSingleton<IMongoDBSettings>(sp =>
                sp.GetRequiredService<IOptions<MongoDBSettings>>().Value);

            services.AddSingleton<MongoClient>(sp =>
                new MongoClient(Configuration.GetValue<string>("MongoDBSettings:ConnectionString")));

            services.AddScoped<IMarketService, MarketService>();

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MarketApi", Version = "v1" });
                c.CustomSchemaIds(x => x.FullName);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("../swagger/v1/swagger.json", "MarketApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthentication();

            //app.UseAuthorization();
            app.UseCors(_policyName);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }

}