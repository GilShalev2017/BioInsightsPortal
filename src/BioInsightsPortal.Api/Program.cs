using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// This registers the services needed for controller-based API endpoints.
builder.Services.AddControllers();
// Add services for API endpoint exploration and Swagger/OpenAPI documentation.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
// In a development environment, we want to enable the Swagger UI for testing.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// This line maps the controller routes to the application.
// It tells ASP.NET Core where to look for the API endpoints you defined in your controllers.
app.MapControllers();

// Starts the application.
app.Run();
