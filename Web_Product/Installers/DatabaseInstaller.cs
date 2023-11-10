using Microsoft.EntityFrameworkCore;
using Web_Product.Models;

namespace Web_Product.Installers
{
    public class DatabaseInstaller : IInstallers
    {
        public void InstallServices(WebApplicationBuilder builder)
        {
            //------------------------ 
            // builder.Configuration.GetConnectionString เอาไว้อ่านค่าใน appsettings
            // Services มาจาก WebApplicationBuilder
            var connectionString = builder.Configuration.GetConnectionString("StockContext");
            builder.Services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(connectionString)
            );
            //------------------------
        }
    }
}
