using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MoneyTransfer.Security;
using ParkAssist.API.Context;
using ParkAssist.API.Models.DTO;
using ParkAssist.API.Models.Entities;
using ParkAssist.API.Models.Mappers;
using ParkAssist.API.Models.Validation;
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
    .AddPolicy("customerpolicy", policy => policy.RequireRole("customer", "valet", "owner"))
    .AddPolicy("valetpolicy", policy => policy.RequireRole("valet", "owner"))
    .AddPolicy("ownerpolicy", policy => policy.RequireRole("owner"));

builder.Services.AddDbContext<ParkAssistContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services
    .AddSingleton<ITokenGenerator>(tk => new JwtGenerator(jwtSecret))
    .AddSingleton<IPasswordHasher>(ph => new PasswordHasher());

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/Customer/{id:int}", async Task<Results<Ok<CustomerDTO>, NotFound>> (ParkAssistContext context, int id) =>
    EntityToDTOMappers.MapCustomer((await context.Customers.SingleOrDefaultAsync(customer => customer.CustomerId == id))!)
        is CustomerDTO customer ? TypedResults.Ok(customer) : TypedResults.NotFound()
).RequireAuthorization("customerpolicy");

app.MapGet("/Customer", async Task<Results<Ok<IEnumerable<CustomerDTO>>, NotFound>> (ParkAssistContext context) =>
    EntityToDTOMappers.MapCustomers(await context.Customers.ToListAsync())
        is IEnumerable<CustomerDTO> customers ? TypedResults.Ok(customers) : TypedResults.NotFound()
).RequireAuthorization("customerpolicy");

app.MapPost("/LogIn", async Task<Results<BadRequest<string>, UnauthorizedHttpResult, Created<UserDTO>>> (ParkAssistContext context, IPasswordHasher passwordHasher, ITokenGenerator tokenGenerator, LogInUserDTO logInUser) =>
{
    (bool IsValid, string ErrorMessage) = DTOValidators.LogInUserDTOIsValid(logInUser);
    if (logInUser is null || !IsValid) { return TypedResults.BadRequest(ErrorMessage); }

    User existingUser = (await context.Users.SingleOrDefaultAsync(user => user.Username == logInUser.Username))!;

    if (existingUser == null || existingUser.UserId < 1) { return TypedResults.BadRequest("matching user not found"); }

    if (!passwordHasher.VerifyHashMatch(existingUser.PasswordHash, logInUser.Password, existingUser.Salt))
    {
        return TypedResults.Unauthorized();
    }
    else
    {
        string token = tokenGenerator.GenerateToken(existingUser.UserId, existingUser.Username, logInUser.Role);

        UserDTO returnUser = EntityToDTOMappers.MapUser(existingUser)!;

        returnUser.Token = token;

        return TypedResults.Created($"/User/{returnUser.UserId}", returnUser);
    }
});

app.MapGet("/Owner/{id:int}", async Task<Results<Ok<OwnerDTO>, NotFound>> (ParkAssistContext context, int id) =>
    EntityToDTOMappers.MapOwner((await context.Owners.SingleOrDefaultAsync(owner => owner.OwnerId == id))!)
        is OwnerDTO owner ? TypedResults.Ok(owner) : TypedResults.NotFound()
).RequireAuthorization("ownerpolicy");

app.MapGet("/Owner", async Task<Results<Ok<IEnumerable<OwnerDTO>>, NotFound>> (ParkAssistContext context) =>
    EntityToDTOMappers.MapOwners(await context.Owners.ToListAsync())
        is IEnumerable<OwnerDTO> owners ? TypedResults.Ok(owners) : TypedResults.NotFound()
).RequireAuthorization("ownerpolicy");

app.MapGet("/ParkingLot/{id:int}", async Task<Results<Ok<ParkingLotDTO>, NotFound>> (ParkAssistContext context, int id) =>
    EntityToDTOMappers.MapParkingLot((await context.ParkingLots.SingleOrDefaultAsync(parkingLot => parkingLot.Id == id))!)
        is ParkingLotDTO parkingLot ? TypedResults.Ok(parkingLot) : TypedResults.NotFound()
).RequireAuthorization("customerpolicy");

app.MapGet("/ParkingLot", async Task<Results<Ok<IEnumerable<ParkingLotDTO>>, NotFound>> (ParkAssistContext context) =>
    EntityToDTOMappers.MapParkingLots(await context.ParkingLots.ToListAsync())
        is IEnumerable<ParkingLotDTO> parkingLots ? TypedResults.Ok(parkingLots) : TypedResults.NotFound()
).RequireAuthorization("customerpolicy");

app.MapGet("/ParkingSlip/{id:int}", async Task<Results<Ok<ParkingSlipDTO>, NotFound>> (ParkAssistContext context, int id) =>
    EntityToDTOMappers.MapParkingSlip((await context.ParkingSlips.SingleOrDefaultAsync(parkingSlip => parkingSlip.Id == id))!)
        is ParkingSlipDTO parkingSlip ? TypedResults.Ok(parkingSlip) : TypedResults.NotFound()
).RequireAuthorization("customerpolicy");

app.MapGet("/ParkingSlip", async Task<Results<Ok<IEnumerable<ParkingSlipDTO>>, NotFound>> (ParkAssistContext context) =>
    EntityToDTOMappers.MapParkingSlips(await context.ParkingSlips.ToListAsync())
        is IEnumerable<ParkingSlipDTO> parkingSlips ? TypedResults.Ok(parkingSlips) : TypedResults.NotFound()
).RequireAuthorization("customerpolicy");

app.MapPost("/Register", async Task<Results<BadRequest<string>, Conflict<string>, Created<UserDTO>>> (ParkAssistContext context, IPasswordHasher passwordHasher, RegisterUserDTO registerUser) =>
{
    (bool IsValid, string ErrorMessage) = DTOValidators.RegisterUserDTOIsValid(registerUser);
    if (registerUser is null || !IsValid) { return TypedResults.BadRequest(ErrorMessage); }

    User existingUser = (await context.Users.SingleOrDefaultAsync(user => user.Username == registerUser.Username))!;
    if (existingUser != null && existingUser.UserId > 0)
    {
        return TypedResults.Conflict("username already taken; choose a different username");
    }
    existingUser = (await context.Users.SingleOrDefaultAsync(user => user.Email == registerUser.Email))!;
    if (existingUser != null && existingUser.UserId > 0)
    {
        return TypedResults.Conflict("email already taken; choose a different email");
    }
    existingUser = (await context.Users.SingleOrDefaultAsync(user => user.Phone == registerUser.Phone))!;
    if (existingUser != null && existingUser.UserId > 0)
    {
        return TypedResults.Conflict("phone already taken; choose a different phone");
    }

    PasswordHash hash = passwordHasher.ComputeHash(registerUser.Password);

    User addUser = DTOToEntityMappers.MapRegisterUser(registerUser)!;

    if (registerUser.Role == "customer")
    {
        addUser.Customer = new();
    }

    else if (registerUser.Role == "valet")
    {
        addUser.Valet = new();

        ParkingLot parkingLot = (await context.ParkingLots.SingleOrDefaultAsync(parkingLot => parkingLot.Id == registerUser.ParkingLotId))!;
        if (parkingLot != null)
        {
            addUser.Valet.ParkingLot = parkingLot;
        }
    }

    else if (registerUser.Role == "owner")
    {
        addUser.Owner = new();
    }

    context.Users.Add(addUser);
    await context.SaveChangesAsync();

    UserDTO returnUser = EntityToDTOMappers.MapUser(addUser)!;
    return TypedResults.Created($"/User/{returnUser.UserId}", returnUser);
});

app.MapGet("/User/{id:int}", async Task<Results<Ok<UserDTO>, NotFound>> (ParkAssistContext context, int id) =>
    EntityToDTOMappers.MapUser((await context.Users.SingleOrDefaultAsync(user => user.UserId == id))!)
        is UserDTO user ? TypedResults.Ok(user) : TypedResults.NotFound()
).RequireAuthorization("customerpolicy");

app.MapGet("/User", async Task<Results<Ok<IEnumerable<UserDTO>>, NotFound>> (ParkAssistContext context) =>
    EntityToDTOMappers.MapUsers(await context.Users.ToListAsync())
        is IEnumerable<UserDTO> users ? TypedResults.Ok(users) : TypedResults.NotFound()
).RequireAuthorization("customerpolicy");

app.MapGet("/Valet/{id:int}", async Task<Results<Ok<ValetDTO>, NotFound>> (ParkAssistContext context, int id) =>
    EntityToDTOMappers.MapValet((await context.Valets.SingleOrDefaultAsync(valet => valet.ValetId == id))!)
        is ValetDTO valet ? TypedResults.Ok(valet) : TypedResults.NotFound()
).RequireAuthorization("valetpolicy");

app.MapGet("/Valet", async Task<Results<Ok<IEnumerable<ValetDTO>>, NotFound>> (ParkAssistContext context) =>
    EntityToDTOMappers.MapValets(await context.Valets.ToListAsync())
        is IEnumerable<ValetDTO> valets ? TypedResults.Ok(valets) : TypedResults.NotFound()
).RequireAuthorization("valetpolicy");

app.MapGet("/Vehicle/{id:int}", async Task<Results<Ok<VehicleDTO>, NotFound>> (ParkAssistContext context, int id) =>
    EntityToDTOMappers.MapVehicle((await context.Vehicles.SingleOrDefaultAsync(vehicle => vehicle.Id == id))!)
        is VehicleDTO vehicle ? TypedResults.Ok(vehicle) : TypedResults.NotFound()
).RequireAuthorization("customerpolicy");

app.MapGet("/Vehicle", async Task<Results<Ok<IEnumerable<VehicleDTO>>, NotFound>> (ParkAssistContext context) =>
    EntityToDTOMappers.MapVehicles(await context.Vehicles.ToListAsync())
        is IEnumerable<VehicleDTO> vehicles ? TypedResults.Ok(vehicles) : TypedResults.NotFound()
).RequireAuthorization("customerpolicy");

app.MapGet("/Customer/{customerId:int}/Vehicle/{vehicleId:int}", async Task<Results<Ok<VehicleDTO>, NotFound>> (ParkAssistContext context, int customerId, int vehicleId) =>
    EntityToDTOMappers.MapVehicle((await context.Vehicles.SingleOrDefaultAsync(vehicle => vehicle.Id == vehicleId))!)
        is VehicleDTO vehicle ? TypedResults.Ok(vehicle) : TypedResults.NotFound()
).RequireAuthorization("customerpolicy");

app.MapGet("/Customer/{customerId:int}/Vehicle", async Task<Results<Ok<IEnumerable<VehicleDTO>>, NotFound>> (ParkAssistContext context, int customerId) =>
    EntityToDTOMappers.MapVehicles(await context.Vehicles.Where(vehicle => vehicle.CustomerId == customerId).ToListAsync())
        is IEnumerable<VehicleDTO> vehicles ? TypedResults.Ok(vehicles) : TypedResults.NotFound()
).RequireAuthorization("customerpolicy");

app.MapPost("/Customer/{customerId:int}/Vehicle", async (ParkAssistContext context, int customerId, VehicleDTO vehicle) =>
{ }
).RequireAuthorization("customerpolicy");

app.MapPut("/Customer/{customerId:int}/Vehicle/{vehicleId:int}", async (ParkAssistContext context, int customerId, int vehicleId, VehicleDTO vehicle) =>
{ }
).RequireAuthorization("customerpolicy");

app.MapDelete("/Customer/{customerId:int}/Vehicle/{vehicleId:int}", async (ParkAssistContext context, int customerId, int vehicleId) =>
{ }
).RequireAuthorization("customerpolicy");


app.Run();
