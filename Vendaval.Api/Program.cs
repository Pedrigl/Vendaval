using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Vendaval.Application.DependencyInjection;
using Vendaval.Application.Services;
using Vendaval.Application.Web;
using Vendaval.Infrastructure.Data.Contexts;
using Vendaval.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(AutoMapping));
builder.Services.AddRepositories();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddServices();

builder.Services.AddDbContexts(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
               builder =>
               {
            builder.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

var app = builder.Build();

app.UseWebSockets();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapHub<ChatHub>("api/chathub");
app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
