using crm_api.Date;
using crm_api.Models.util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace crm_api
{
    public class Startup
    {
        public static IWebHostEnvironment _environment;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
        }
      /*  
        public Startup()
        {
            
        }*/

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddDbContext<Crm_DbContext>(option => option.UseMySql(Configuration.GetConnectionString("MySql")
                , new MySqlServerVersion(new Version(8, 0, 27)
                )));
            services.AddControllersWithViews().AddNewtonsoftJson
               (options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddCors(option =>
                option.AddPolicy("CorsPolicy", p =>
                 p.AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowAnyOrigin()
                ));
            services.Configure<FormOptions>(options =>
            {
                // Set the limit to 256 MB
                options.MultipartBodyLengthLimit = 268435456;
            });
            /*   services.AddMvc(options =>
                {   //加入返回结果处理
                    options.Filters.Add<ApiResultFilter>();
                });*/
            /*     services.AddMvc(options =>
                  {
                      options.Filters.Add(typeof(WebApiResultMiddleware));
                      options.RespectBrowserAcceptHeader = true;
                  });*/

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
              /*  app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "crm_api v1"));*/
            }
            if (!Directory.Exists(env.ContentRootPath + "/api/Images"))
            {
                Directory.CreateDirectory(env.ContentRootPath + "/api/Images");
            }
             //添加一个访问静态文件的中间件
                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "api")),
                    RequestPath = "/api"
                });
            
     
           
        
            app.UseHttpsRedirection();

            app.UseRouting();

           app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
           
        }
    }
}
