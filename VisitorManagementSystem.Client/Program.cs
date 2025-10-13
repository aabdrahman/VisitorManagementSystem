using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using VisitorManagementSystem.Client;
using VisitorManagementSystem.Client.AuthenticationProvider;
using VisitorManagementSystem.Presentation.Helpers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddTransient<AuthStateHandler>();
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddHttpClient(ClientHelper.OpenClientKey, opts =>
{
    opts.BaseAddress = new Uri(ClientHelper.BaseUri);
});

builder.Services.AddHttpClient(ClientHelper.SecureClientKey, opts =>
{
    opts.BaseAddress = new Uri(ClientHelper.BaseUri);

}).AddHttpMessageHandler<AuthStateHandler>();

await builder.Build().RunAsync();
