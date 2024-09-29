using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Serilog;
using Contact_Manager.Filters.ActionFilters;
using Contact_Manager;
using Contact_Manager.Middleware;

var builder = WebApplication.CreateBuilder(args);

//Serilog
builder.Host.UseSerilog((HostBuilderContext context,
    IServiceProvider services, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration
    .ReadFrom.Configuration(context.Configuration) //read configuration settings from built-in IConfiguration
    .ReadFrom.Services(services); //read out current app's services and make them available to serilog
});

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseExceptionHandlingMiddleware();
}

Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
//Rotativa.AspNetCore.RotativaConfiguration.Setup(builder.Environment.WebRootPath, "Rotativa");
//RotativaConfiguration.Setup(builder.Environment.WebRootPath, "Rotativa");


//Enabling Https
app.UseHsts();
app.UseHttpsRedirection();

//Logging
app.UseHttpLogging();

//using wwwroot folder
app.UseStaticFiles();

app.UseRouting();//Identifying action method based route
app.UseAuthentication();//Reading Identity cookie
app.UseAuthorization(); //evaluation access permission of the current user
app.MapControllers();//Execute the filter pipeline (action + filters)

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller}/{action}/{id?}");
    //admin/home/Index



app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}/{id?}");

app.UseRotativa();

app.Run();

public partial class Program { } //make the auto-generated Program accessible programmatically