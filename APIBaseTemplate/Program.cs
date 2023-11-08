using APIBaseTemplate.Common;
using APIBaseTemplate.Repositories;
using APIBaseTemplate.Services;
using Microsoft.OpenApi.Models;

var apiVersion = Constants.API_BASE_TEMPLATE_APPLICATION_VERSION;
var apiName = Constants.API_BASE_TEMPLATE_APPLICATION_NAME;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new ProducesResponseTypeConvention());
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        apiVersion,
        new OpenApiInfo()
        {
            Title = apiName,
            Version = apiVersion
        });
});
builder.Services.AddAPIBaseTemplateDbContext(builder.Configuration);


builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ICityBusiness, CityBusiness>();

builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<IRegionBusiness, RegionBusiness>();

builder.Services.AddScoped<IAirlineRepository, AirlineRepository>();
builder.Services.AddScoped<IAirlineBusiness, AirlineBusiness>();

builder.Services.AddScoped<IAirportRepository, AirportRepository>();
builder.Services.AddScoped<IAirportBusiness, AirportBusiness>();

builder.Services.AddScoped<IFligthRepository, FligthRepository>();
builder.Services.AddScoped<IFligthBusiness, FligthBusiness>();

builder.Services.AddScoped<IFligthServiceRepository, FligthServiceRepository>();
builder.Services.AddScoped<IFligthServiceBusiness, FligthServiceBusiness>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint($"./{apiVersion}/swagger.json", apiName);
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
