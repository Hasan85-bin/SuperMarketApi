using Microsoft.Extensions.DependencyInjection;
using SuperMarketApi.Mapping;
using SuperMarketApi.Services;
using SuperMarketApi.Filters;

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
builder.Services.AddSingleton<IProductService, ProductService>();
builder.Services.AddSingleton<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// app.UseCors("AllowAll");



app.MapControllers();

app.Run();
