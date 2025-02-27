using IMS.Application;
using IMS.Application.Services;
using IMS.Domain.Interfaces;
using IMS.Infrastructure;
using IMS.Infrastructure.DbContext;
using IMS.Infrastructure.ExternalServices;
using IMS.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region Connect To SQL Server
var ConnectionString = builder.Configuration.GetConnectionString("DefultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(option =>
option.UseSqlServer(ConnectionString)
);
#endregion

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 204857600; // ÍÌã ÇáãáÝ ÇáãÓãæÍ Èå (100MB)
});



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
});

#region DependencyInjection Settings
builder.Services.AddInfrastructureDependencis();
builder.Services.AddApplicationDependencis().AddServiceRegistration(builder.Configuration);
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IAuthenticationRepository, AuthenticationRepository>();
builder.Services.AddTransient<IUserRefreshTokenRepository, UserRefreshTokenRepository>();
builder.Services.AddScoped<EmailService>();

//For Request info
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddTransient<IUrlHelper>(x =>
{
    var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
    var factory = x.GetRequiredService<IUrlHelperFactory>();
    return factory.GetUrlHelper(actionContext);
});


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

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
