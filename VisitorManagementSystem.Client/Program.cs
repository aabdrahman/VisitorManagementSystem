using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using VisitorManagementSystem.Client;
using VisitorManagementSystem.Client.AuthenticationProvider;
using VisitorManagementSystem.Client.Handler.Authentication;
using VisitorManagementSystem.Client.Handler.UserManagement;
using VisitorManagementSystem.Client.Handler.Visit;
using VisitorManagementSystem.Client.Handler.Visitor;
using VisitorManagementSystem.Client.Helpers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddTransient<AuthStateHandler>();
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<AuthenticationSigninHandler>();
builder.Services.AddScoped<RefreshTokenHandler>();
builder.Services.AddScoped<GetVisitsHandler>();
builder.Services.AddScoped<CheckinHandler>();
builder.Services.AddScoped<CheckoutHandler>();
builder.Services.AddScoped<GetVisitorsHandler>();
builder.Services.AddScoped<UpdateVisitorHandler>();
builder.Services.AddScoped<GetRolesHandler>();
builder.Services.AddScoped<GetUsersHandler>();
builder.Services.AddScoped<AddRoleHandler>();
builder.Services.AddScoped<RegisterUserHandler>();
builder.Services.AddScoped<AuthenticationSignoutHandler>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddHttpClient(ClientHelper.OpenClientKey, opts =>
{
    opts.BaseAddress = new Uri(ClientHelper.BaseUri);
});

builder.Services.AddHttpClient(ClientHelper.SecureClientKey, opts =>
{
    opts.BaseAddress = new Uri(ClientHelper.BaseUri);
    opts.Timeout = TimeSpan.FromSeconds(10);

}).AddHttpMessageHandler<AuthStateHandler>();

await builder.Build().RunAsync();
