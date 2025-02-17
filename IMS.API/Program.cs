using IMS.Application;
using IMS.Application.Services;
using IMS.Infrastructure;
using IMS.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region Connect To SQL Server
var ConnectionString = builder.Configuration.GetConnectionString("DefultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(option =>
option.UseSqlServer(ConnectionString)
);
#endregion


#region DependencyInjection Settings
builder.Services.AddInfrastructureDependencis();
builder.Services.AddApplicationDependencis();

builder.Services.AddTransient<ProductsServices>();
#endregion
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
