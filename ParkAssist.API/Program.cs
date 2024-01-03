using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MoneyTransfer.Security;
using ParkAssist.API.Context;
using ParkAssist.API.Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
            .Build();

string connectionString = config.GetConnectionString("Project") ?? "Error retrieving connection string!";
string jwtSecret = config.GetValue<string>("JwtSecret") ?? "Error retreiving jwt config!";

byte[] key = Encoding.ASCII.GetBytes(jwtSecret);
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap[JwtRegisteredClaimNames.Sub] = "sub";

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        NameClaimType = "name"
    };
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("requireauthuser", policy => policy.RequireAuthenticatedUser());

builder.Services.AddDbContext<ParkAssistContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services
    .AddSingleton<ITokenGenerator>(tk => new JwtGenerator(jwtSecret))
    .AddSingleton<IPasswordHasher>(ph => new PasswordHasher());

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/Customer/{id:int}", async (int id, ParkAssistContext context) =>
{
    return (await context.Customers.SingleOrDefaultAsync(customer => customer.CustomerId == id))!;
    
});

app.Run();
