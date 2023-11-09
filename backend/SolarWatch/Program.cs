using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SolarWatch;
using SolarWatch.Data;
using SolarWatch.Services;
using SolarWatch.Services.Authentication;

var builder = WebApplication.CreateBuilder(args);

//Reading the appsettings.json file
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();

//creating variables for config strings
var appSettings = configuration.GetSection("AppSettings");
var validIssuer = appSettings["ValidIssuer"];
var validAudience = appSettings["ValidAudience"];
//var issuerSigningKey = builder.Configuration["UserSecrets:IssuerSigningKey"];
//var databaseConnectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
//var geocodingApiKey = builder.Configuration["GeocodingApi:ApiKey"];

var issuerSigningKey = Environment.GetEnvironmentVariable("IssuerSigningKey");
var databaseConnectionString = Environment.GetEnvironmentVariable("DefaultConnection");

AddServices();
ConfigureSwagger();
AddDbContext();
AddAuthentication();
AddIdentity();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

/*
using var db = new SolarWatchContext(new DbContextOptionsBuilder<SolarWatchContext>().UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]).Options, configuration);
PrintCities();

void PrintCities() //reads all the data from the Cities DbSet and prints them on the console
{
    foreach (var city in db.Cities)
    {
        Console.WriteLine($"{city.Id}, {city.Name}, Latitude: {city.Lat}, Longitude: {city.Lon}");
    }
}
*/

AddRoles();
AddAdmin();

app.Run();


void AddServices()
{
    // Add services to the container.
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSingleton<IGeocodeDataProvider, GeocodingApi>();
    builder.Services.AddSingleton<ICityJsonProcessor, CityJsonProcessor>();
    builder.Services.AddSingleton<ISunriseAndSunsetDataProvider, SunriseAndSunsetApi>();
    builder.Services.AddSingleton<ISunriseAndSunsetJsonProcessor, SunriseAndSunsetJsonProcessor>();

    //register the repository interfaces and implementations:
    builder.Services.AddSingleton<ICityRepository>(provider => new CityRepository(configuration, databaseConnectionString));
    builder.Services.AddSingleton<ISunriseAndSunsetRepository>(provider => new SunriseAndSunsetRepository(configuration, databaseConnectionString));
    
    //register the new application services:
    builder.Services.AddScoped<IAuthService, AuthService>();

    //add the TokenService as a scoped service:
    builder.Services.AddScoped<ITokenService>(provider => new TokenService(issuerSigningKey!, validIssuer!, validAudience!));
    
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins", builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    });
}

void ConfigureSwagger()
{
    builder.Services.AddSwaggerGen(
        option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "SolarWatch 6", Version = "v1" });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        });
}

void AddDbContext()
{
    //register the new UsersContext as a DbContext:
    builder.Services.AddDbContext<UsersContext>();
    builder.Services.AddDbContext<SolarWatchContext>();
}

void AddAuthentication()
{
    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {

            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ClockSkew = TimeSpan.Zero,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = validIssuer, //changing the config strings
                ValidAudience = validAudience, //changing the config strings
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(issuerSigningKey)
                ),
            };
        });
}

void AddIdentity()
{
    //this specifies the requirements for new user registrations:
    builder.Services
        .AddIdentityCore<IdentityUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        })
        .AddRoles<IdentityRole>() //Enable Identity roles 
        .AddEntityFrameworkStores<UsersContext>();
}

void AddRoles()
{
    using var scope = app.Services.CreateScope(); // RoleManager is a scoped service, therefore we need a scope instance to access it
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var tAdmin = CreateAdminRole(roleManager);
    tAdmin.Wait();

    var tUser = CreateUserRole(roleManager);
    tUser.Wait();
}

async Task CreateAdminRole(RoleManager<IdentityRole> roleManager)
{
    await roleManager.CreateAsync(new IdentityRole("Admin")); //The role string should better be stored as a constant or a value in appsettings
}

async Task CreateUserRole(RoleManager<IdentityRole> roleManager)
{
    await roleManager.CreateAsync(new IdentityRole("User")); //The role string should better be stored as a constant or a value in appsettings
}

void AddAdmin()
{
    var tAdmin = CreateAdminIfNotExists();
    tAdmin.Wait();
}

async Task CreateAdminIfNotExists()
{
    using var scope = app.Services.CreateScope();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var adminInDb = await userManager.FindByEmailAsync("admin@admin.com");
    if (adminInDb == null)
    {
        var admin = new IdentityUser { UserName = "admin", Email = "admin@admin.com" };
        var adminCreated = await userManager.CreateAsync(admin, "admin123");

        if (adminCreated.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}