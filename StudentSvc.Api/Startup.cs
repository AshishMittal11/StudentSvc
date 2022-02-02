using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StudentSvc.Api.Configuration;
using StudentSvc.Api.Database;
using MediatR;
using System.Reflection;
using StudentSvc.Api.Azure;
using StudentSvc.Api.Models;
using StudentSvc.Api.Repository;
using StudentSvc.Api.Cosmos;

namespace StudentSvc.Api
{
    public class Startup
    {
        private readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<StudentContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("StudentDBConnectionString"));
            });

            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);

            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("https://localhost:5000");
                                      builder.AllowAnyMethod();
                                      builder.AllowAnyHeader();
                                      builder.AllowAnyOrigin();
                                  });
            });

            services.AddSwaggerGen();

            services.AddHttpClient();

            // this is for sending the data to the cosmos db....
            services.Configure<CosmosSettings>(Configuration.GetSection("CosmosSettings"));

            services.Configure<ExternalServices>(Configuration.GetSection("ExternalServices"));

            // this is for reading the topic, endpoints and subscription from the appsetting file.
            services.Configure<TopicSettings>(Configuration.GetSection("TopicSettings"));

            services.AddScoped<IStudentRepository, StudentRepository>();

            // this is the wrapper class which will send the message to the topic which internally will send the message to its corresponding subscriptions.
            services.AddScoped<ServiceBusTopicSender>();

            // this is the consumer of service bus thrown messages.
            services.AddSingleton<IServiceBusConsumer, ServiceBusConsumer>();

            // this will process the message and extract the content and url and will throw the content at the mentioned url.
            services.AddSingleton<IProcessData, ProcessData>();

            // hosting this service to run in the background, this is acting as listener to the subscription.....
            services.AddHostedService<WorkerServiceBus>();

            // setting up the application insights for logging and tracing...
            services.AddApplicationInsightsTelemetry();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // mapter related configuration done here...
            StudentConfiguration.Configure();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint.  
            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),  
            // specifying the Swagger JSON endpoint.  
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Student Service");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
