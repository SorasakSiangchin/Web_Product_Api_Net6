namespace Web_Product.Installers
{
    public interface IInstallers
    {
        // WebApplicationBuilder คือ WebApplication.CreateBuilder(args); ใน Program.cs
        void InstallServices(WebApplicationBuilder builder);
    }
}
