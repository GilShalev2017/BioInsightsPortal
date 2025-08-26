//using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Components.Web;
//using BioInsightsPortal.Web.Data;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddRazorPages();
//builder.Services.AddServerSideBlazor();
//builder.Services.AddSingleton<WeatherForecastService>();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();

//app.UseStaticFiles();

//app.UseRouting();

//app.MapBlazorHub();
//app.MapFallbackToPage("/_Host");

//app.Run();
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using BioInsightsPortal.Web.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

// Add the BioInsightsService and register HttpClient
builder.Services.AddHttpClient<BioInsightsService>(client =>
{
    // Make sure this URL matches the URL of your running API project.
    // In development, this is typically https://localhost:5142/
    
    //client.BaseAddress = new Uri("https://localhost:5142/"); //PRODUCTION we use https!
    client.BaseAddress = new Uri("http://localhost:5142/");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
