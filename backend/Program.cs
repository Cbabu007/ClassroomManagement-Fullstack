using ClassroomManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;

namespace ClassroomManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ClassroomManagement API",
                    Version = "v1"
                });
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            
            builder.Services.AddDbContext<ClassroomDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ClassroomManagement v1");
                    c.RoutePrefix = "swagger"; 
                });
            }

            app.UseStaticFiles(); 

            
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")), 
                RequestPath = "/uploads"
            });


            app.UseHttpsRedirection();
            app.UseCors();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
