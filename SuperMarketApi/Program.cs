using Microsoft.Extensions.DependencyInjection;
using SuperMarketApi.Mapping;
using SuperMarketApi.Services;
using SuperMarketApi.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SuperMarketApi;
using SuperMarketApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services
    .AddControllers()
    .AddNewtonsoftJson();

// Add AutoMapper with proper profile registration
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add CORS
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowAll", policy =>
//     {
//         policy.AllowAnyOrigin()
//               .AllowAnyMethod()
//               .AllowAnyHeader();
//     });
// });

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "SuperMarket API",
        Version = "v1",
        Description = "A simple API for managing supermarket products and customers"
    });
    
    // Configure enum values to show as strings in Swagger
    //c.UseInlineDefinitionsForEnums();
    
    // Add schema filter to show enum values
    c.SchemaFilter<EnumSchemaFilter>();
});

// Register services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IPurchaseCartRepository, PurchaseCartRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICartPurchaseService, CartPurchaseService>();
builder.Services.AddScoped<IStaffService, StaffService>();

// Add distributed memory cache for session support
builder.Services.AddDistributedMemoryCache();

// Add session with 5-minute timeout
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add this in the builder.Services section:
builder.Services.AddDbContext<SuperMarketDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// JWT configuration values (for demo purposes, use secure storage in production)
// IMPORTANT: The key must be at least 32 characters (256 bits) for HS256
var jwtKey = "YourSuperSecretKey1234567890!@#$%^"; // 32+ chars
var jwtIssuer = "SuperMarketApi";
var jwtAudience = "SuperMarketApiUsers";

// Add JWT authentication
builder.Services.AddAuthentication(options =>
{
    // Set the default authentication scheme to JWT Bearer
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Configure JWT Bearer options
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ClockSkew = TimeSpan.Zero // No clock skew for demo
    };
});

builder.Services.AddHttpContextAccessor();


var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable session middleware before authentication/authorization
app.UseSession();

// Enable authentication and authorization middleware
app.UseAuthentication(); // Must come before UseAuthorization
app.UseAuthorization();

// app.UseCors("AllowAll");



app.MapControllers();

app.Run();
