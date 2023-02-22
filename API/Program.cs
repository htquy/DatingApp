using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
            /*tạo một webhost bằng cách load dữ liệu các file json: appsetting.json,appsettings.Development.json,các
            biến môi trường, các tham số truyền vào -> build->chạy webhost với Kestrel*/
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    //báo cho host bt là class nào sẽ gọi khi start host. và host sẽ tìm kiếm phần cài đặt trong 2 phương thức Configure và ConfigureServices.
                    
                });
    }
}
