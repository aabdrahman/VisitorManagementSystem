using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using NLog;
using VisitorManagementSystem.Extensions;

var builder = WebApplication.CreateBuilder(args);

LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
builder.Services.ConfigureSwagger();

builder.Services.ConfigureLogggerService();
builder.Services.ConfigureExceptionHandler();
//builder.Services.ConfigureSqlConnection(builder.Configuration);
builder.Services.ConfigureDbConnection(builder.Configuration);
builder.Services.ConfigureIdentityContext();
builder.Services.ConfigureRepository();
builder.Services.ConfigureAutoMapper();
builder.Services.ConfigureServices();
builder.Services.ConfigureCors();
//builder.Services.ConfigureCors();
builder.Services.ConfigureActionFilters();
builder.Services.ConfigureJwt(builder.Configuration);
builder.Services.ConfigureJwtAuthorization();
//builder.Services.ConfigureOutputCaching();
builder.Services.ConfigureResponseCaching();
builder.Services.ConfigureRateLimiting();
builder.Services.ConfigureEnumSerializer();

// Add services to the container.

builder.Services.AddControllers(o =>
{
    o.CacheProfiles.Add("300Seconds", new CacheProfile { Duration = 300, Location = ResponseCacheLocation.Any });
}).AddApplicationPart(typeof(VisitorManagementSystem.Presentation.AssemblyReference).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI(opts =>
{
    opts.SwaggerEndpoint("/swagger/v1/swagger.json", "Visitor Management System API");
    //opts.RoutePrefix = "VisitorManagementSystemApi";
});

app.UseExceptionHandler(opts => { });

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseRateLimiter();
app.UseCors("defaultPolicy");
//app.UseOutputCache();
app.UseResponseCaching();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
