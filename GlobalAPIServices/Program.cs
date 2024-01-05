using GlobalAPIServices.Extension;
using GlobalAPIServices.Services.UserService;
using GlobalAPIServices.LoggerService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using Microsoft.EntityFrameworkCore;
using GlobalAPIServices.Application.Service;
using GlobalAPIServices.Application.Impl;
using GlobalAPIServices.Infrastracture.Repository;
using GlobalAPIServices.Infrastracture.Repository.Impl;
using GlobalAPIServices.Security;
using GlobalAPIServices.Infrastracture.Repository.UserManagementData;

var builder = WebApplication.CreateBuilder(args);

//nLog configuratioin

#pragma warning disable CS0618 // Type or member is obsolete
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
#pragma warning restore CS0618 // Type or member is obsolete

// Add services to the container.
builder.Services.AddSingleton<ILoggerManager, LoggerManager>();

//builder.Services.ConfigureLoggerService();

// Add DbContext services to the container
builder.Services.AddDbContext<UserManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UserManagement_DbContext") ?? throw new InvalidOperationException("Connection string 'UserManagement_DbContext' not found.")));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen(opt=>
{
    opt.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description="Standard Authorazation bearer {token}",
        In=ParameterLocation.Header,
        Name="Authorization",
        Type=SecuritySchemeType.ApiKey
    });
    opt.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

//builder.Services.AddSession();

#region Internal Service

builder.Services.AddScoped<IUserService, UserService>();

#endregion Internal Service

#region Internal Security Service

builder.Services.AddScoped<IPasswordManager,PasswordManager>();
builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();
builder.Services.AddScoped<ITokenDecoder, TokenDecoder>();

#endregion Internal Security Service

#region DB Data
builder.Services.AddScoped<IUserApplicationService, UserApplicationService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

#endregion DB Data

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseSession();

app.ConfigureCustomExceptionMiddleware();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
