using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MoneyTransfer.Security;
using ParkAssist.API.Context;
using ParkAssist.API.Models.DTO;
using ParkAssist.API.Models.Entities;
using ParkAssist.API.Models.Mappers;
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
    .AddSingleton<IPasswordHasher>(ph => new PasswordHasher())
    .AddSingleton<Mappers>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/Vehicle/{id:int}", async Task<Results<Ok<VehicleDTO>, NotFound>> (ParkAssistContext context, Mappers mapper, int id) =>
    mapper.MapVehicle<Vehicle>((await context.Vehicles.SingleOrDefaultAsync(vehicle => vehicle.Id == id))!)
        is VehicleDTO vehicleDTO ? TypedResults.Ok(vehicleDTO) : TypedResults.NotFound()
);

app.MapGet("/Vehicle", async Task<Results<Ok<IEnumerable<VehicleDTO>>, NotFound>> (ParkAssistContext context, Mappers mapper) =>
    mapper.MapVehicles<IEnumerable<Vehicle>>(await context.Vehicles.ToListAsync())
        is IEnumerable<VehicleDTO> vehicleCollection ? TypedResults.Ok(vehicleCollection) : TypedResults.NotFound()
);

app.MapGet("/Vehicle/User/{userId:int}", async Task<Results<Ok<IEnumerable<VehicleDTO>>, NotFound>> (ParkAssistContext context, Mappers mapper, int userId) =>
    mapper.MapVehicles<IEnumerable<Vehicle>>(await context.Vehicles.Where(vehicle => vehicle.Customer.UserId == userId).ToListAsync())
        is IEnumerable<VehicleDTO> vehicleCollection ? TypedResults.Ok(vehicleCollection) : TypedResults.NotFound()
);

app.MapGet("/Valet/{id:int}", async Task<Results<Ok<ValetDTO>, NotFound>> (ParkAssistContext context, Mappers mapper, int id) =>
    mapper.MapValet<Valet>((await context.Valets.Include(valet => valet.ParkingLot).IgnoreAutoIncludes().Include(valet => valet.User).SingleOrDefaultAsync(valet => valet.ValetId == id))!)
        is ValetDTO valetDTO ? TypedResults.Ok(valetDTO) : TypedResults.NotFound()
);

app.MapGet("/Valet", async Task<Results<Ok<IEnumerable<ValetDTO>>, NotFound>> (ParkAssistContext context, Mappers mapper) =>
    mapper.MapValets<IEnumerable<Valet>>(await context.Valets.Include(valet => valet.ParkingLot).IgnoreAutoIncludes().Include(valet => valet.User).ToListAsync())
        is IEnumerable<ValetDTO> valetCollection ? TypedResults.Ok(valetCollection) : TypedResults.NotFound()
);

app.MapGet("/Valet/ParkingLot/{parkingLotId:int}", async Task<Results<Ok<IEnumerable<ValetDTO>>, NotFound>> (ParkAssistContext context, Mappers mapper, int parkingLotId) =>
    mapper.MapValets<IEnumerable<Valet>>(await context.Valets.Include(valet => valet.ParkingLot).IgnoreAutoIncludes().Include(valet => valet.User).Where(valet => valet.ParkingLotId == parkingLotId).ToListAsync())
        is IEnumerable<ValetDTO> valetCollection ? TypedResults.Ok(valetCollection) : TypedResults.NotFound()
);

app.Run();
