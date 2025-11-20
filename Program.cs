using back_app_par.auth.register.contracts;
using back_app_par.auth.register.repository;
using back_app_par.data;
using back_app_par.middleware.Error;
using back_app_par.test;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Environment.EnvironmentName = "Development";
var cadenaString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine(cadenaString);

builder.Services.AddDbContext<appContext>(o =>
{
    o.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddControllers();

// CONFIGURAR JWT


// IMPLEMENTACIONES
builder.Services.AddScoped<IPruebaConexion, PruebaServices>();
builder.Services.AddScoped<ILogin,Login_repo>();
builder.Services.AddScoped<IRegister, Register_repo>();

builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
//app.UseExceptionHandler("/error");
//app.UseMiddleware<ExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<CustomExceptionHandlerMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

