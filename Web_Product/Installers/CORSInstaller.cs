namespace Web_Product.Installers
{
    public class CORSInstaller : IInstallers
    {
        public static string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public static string MyAllowAnyOrigins = "_myAllowAnyOrigins";
        public void InstallServices(WebApplicationBuilder builder)
        {
           
            #region การอนุญาตสิทธิ์
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                              // policy เป็น นโยบาย
                              policy =>
                              {
                                  policy.WithOrigins("https://www.w3schools.com"//AllowAnyMethod ทุก method เข้ามาใช้ได้หมด                                                                                   
                                                                                //WithMethods("GET","PUT") ใช้ method ได้บางอัน
                                                      ).AllowAnyHeader().AllowAnyMethod();
                              });
            });
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowAnyOrigins,
                              // policy เป็น นโยบาย
                              policy =>
                              {
                                  policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                              });
            });
            #endregion
        }
    }
}
