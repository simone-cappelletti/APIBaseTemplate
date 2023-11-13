using APIBaseTemplate.Common;
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
builder.Services.AddRepositories();
builder.Services.AddBusinessServices();

var app = builder.Build();

app.UseExceptionHandlingMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
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
