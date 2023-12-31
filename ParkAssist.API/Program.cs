using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MoneyTransfer.Security;
using ParkAssist.API.Context;
using ParkAssist.API.Models;
using ParkAssist.API.Models.DTOs;
using ParkAssist.API.Models.Entities;
using ParkAssist.Core;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
            .Build();

string connectionString = config.GetConnectionString("Project") ?? "Error retrieving connection string!";
string jwtSecret = config.GetValue<string>("JwtSecret") ?? "Error retreiving jwt config!";

var key = Encoding.ASCII.GetBytes(jwtSecret);
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
    .AddSingleton<IPasswordHasher>(ph => new PasswordHasher())
    .AddSingleton<Mappers>();

WebApplication app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/Admin/{id:int}", async Task<AdminDTO> (int id, ParkAssistContext context, Mappers mappers) =>
{
    Admin admin = (await context.Admins.SingleOrDefaultAsync(admin => admin.Id == id))!;
    if (GuardClauses.IsNotNull<Admin>(admin))
    {
        AdminDTO dto = mappers.Map<Admin>(admin);
        return dto;
    }
    return AdminDTO.AdminDTONotFound;
});

app.MapGet("/Admin", async Task<IReadOnlyList<AdminDTO>> (ParkAssistContext context, Mappers mappers) =>
    await Task.Run(() =>
        {
            List<AdminDTO> dtos = new();
            foreach (Admin admin in context.Admins)
            {
                if (GuardClauses.IsNotNull<Admin>(admin))
                {
                    AdminDTO dto = mappers.Map<Admin>(admin);
                    dtos.Add(dto);
                }
            }
            return dtos.AsReadOnly();
        }
    )
);

app.MapGet("/Owner/{id:int}", async Task<OwnerDTO> (int id, ParkAssistContext context, Mappers mappers) =>
{
    Owner owner = (await context.Owners.SingleOrDefaultAsync(owner => owner.Id == id))!;
    if (GuardClauses.IsNotNull<Owner>(owner))
    {
        OwnerDTO dto = mappers.Map<Owner>(owner);
        return dto;
    }
    return OwnerDTO.OwnerDTONotFound;
});

app.MapGet("/Owner", async Task<IReadOnlyList<OwnerDTO>> (ParkAssistContext context, Mappers mappers) =>
    await Task.Run(() =>
    {
        List<OwnerDTO> dtos = new();
        foreach (Owner owner in context.Owners)
        {
            if (GuardClauses.IsNotNull<Owner>(owner))
            {
                OwnerDTO dto = mappers.Map<Owner>(owner);
                dtos.Add(dto);
            }
        }
        return dtos.AsReadOnly();
    }
    )
);

app.MapGet("/Customer/{id:int}", async Task<CustomerDTO> (int id, ParkAssistContext context, Mappers mappers) =>
{
    Customer customer = (await context.Customers.SingleOrDefaultAsync(customer => customer.Id == id))!;
    if (GuardClauses.IsNotNull<Customer>(customer))
    {
        CustomerDTO dto = mappers.Map<Customer>(customer);
        return dto;
    }
    return CustomerDTO.CustomerDTONotFound;
});

app.MapGet("/Customer", async Task<IReadOnlyList<CustomerDTO>> (ParkAssistContext context, Mappers mappers) =>
    await Task.Run(() =>
    {
        List<CustomerDTO> dtos = new();
        foreach (Customer customer in context.Customers)
        {
            if (GuardClauses.IsNotNull<Customer>(customer))
            {
                CustomerDTO dto = mappers.Map<Customer>(customer);
                dtos.Add(dto);
            }
        }
        return dtos.AsReadOnly();
    }
    )
);

app.MapGet("/Valet/{id:int}", async Task<ValetDTO> (int id, ParkAssistContext context, Mappers mappers) =>
{
    Valet valet = (await context.Valets.SingleOrDefaultAsync(valet => valet.Id == id))!;
    if (GuardClauses.IsNotNull<Valet>(valet))
    {
        ValetDTO dto = mappers.Map<Valet>(valet);
        return dto;
    }
    return ValetDTO.ValetDTONotFound;
});

app.MapGet("/Valet", async Task<IReadOnlyList<ValetDTO>> (ParkAssistContext context, Mappers mappers) =>
    await Task.Run(() =>
    {
        List<ValetDTO> dtos = new();
        foreach (Valet valet in context.Valets)
        {
            if (GuardClauses.IsNotNull<Valet>(valet))
            {
                ValetDTO dto = mappers.Map<Valet>(valet);
                dtos.Add(dto);
            }
        }
        return dtos.AsReadOnly();
    }
    )
);

app.Run();
