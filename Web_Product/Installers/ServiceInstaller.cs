using Autofac;
using Autofac.Extensions.DependencyInjection;
using System.Reflection;

namespace Web_Product.Installers
{
    public class ServiceInstaller : IInstallers
    {
        public void InstallServices(WebApplicationBuilder builder)
        {
            //----------------------- จะได้ไม่ต้องลงทะเบียนหลายอัน ---------------------------
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory(containerBuilder =>
            {
                containerBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                //- จะค้นหาชื่อไฟล์ที่ลงท้ายด้วย Service แล้วจะ DI โดยอัตโนมัติ
                .Where(t => t.Name.EndsWith("Service") || t.Name.EndsWith("Test"))
                .AsImplementedInterfaces();
            }));
            // สร้างเพียงหนึ่งครั้งเท่านั้น refresh ค่าก้ไม่เปลี่ยน
            //builder.Services.AddSingleton<IProductService, ProductService>();
            // สร้างเพียงครั้งเดียว
            //builder.Services.AddScoped<IProductService, ProductService>();
            // สร้างหลายครั้ง
            //builder.Services.AddTransient<IProductService, ProductService>();
        }
    }
}
