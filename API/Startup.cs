using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using API.Data;
using Microsoft.EntityFrameworkCore;
using API.interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using API.Extensions;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {   services.AddApplicationServices(_config);
            services.AddScoped<ITokenService,TokenService>();
            services.AddDbContext<DataContext> (options => {
                options.UseSqlite(_config.GetConnectionString("DefaultConnection"));//ket noi voi database sqlite va ket noi toi chuoi "DefaultConnection"
            });
            services.AddControllers();
            services.AddCors();//cho phep su dung chia du lieu tu API server 
            services.AddIdentityServices(_config);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options=>
            {
                options.TokenValidationParameters=new TokenValidationParameters
                {
                    ValidateIssuerSigningKey=true,
                    IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokenkey"])),
                    ValidateIssuer=false,
                    ValidateAudience=false
                };
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPIv5", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())//kiem tra xem moi truong host co phai la Microsoft.Extensions.Hosting.EnvironmentName.Development ko
            {
                app.UseDeveloperExceptionPage();// tao phan hoi loi den trang html
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIv5 v1"));
            }

            app.UseHttpsRedirection();//truy cập và chuyển hướng http đến điểm cuối TPS.

            app.UseRouting();//truy cập web qua định tuyến
            app.UseCors(x=>x.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));//cho phep truy cap du lieu tu http://localhost:5193 
            //toi http://localhost:4200
            app.UseAuthentication();//xac minh quyen truy cap
            app.UseAuthorization();// uy quyen truy cap

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();// lien ket cac diem cuoi cua http
            });
        }
    }
}
