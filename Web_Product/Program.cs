using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Web_Product.Installers;
using Web_Product.interfaces;
using Web_Product.Models;
using Web_Product.Services;

// การลงทะเบียนคือการสร้าง opject
var builder = WebApplication.CreateBuilder(args);

builder.Services.MyInstallerExtensions(builder);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();










//======================================================================================================




    
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseCors(MyAllowSpecificOrigins);
app.UseCors(CORSInstaller.MyAllowAnyOrigins);
//ถ้าใช้หลายๆอันให้พิมพ์เพิ่มไปอีก
//app.UseCors(MyAllowSpecificOrigins);



app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.MapControllers();

app.UseStaticFiles();






app.Run();
