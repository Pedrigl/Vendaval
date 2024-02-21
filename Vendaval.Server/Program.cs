using Microsoft.EntityFrameworkCore;
using Vendaval.Application.DependencyInjection;
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

builder.Services.AddDbContextPool<VendavalDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("VendavalDb"));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
