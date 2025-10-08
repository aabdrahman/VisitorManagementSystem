using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using VisitorManagementSystem.SelfServiceUI;
using VisitorManagementSystem.SelfServiceUI.AuthProvider;
using VisitorManagementSystem.SelfServiceUI.Handlers.Authentication;
using VisitorManagementSystem.SelfServiceUI.Handlers.VisitDetail;
using VisitorManagementSystem.SelfServiceUI.Handlers.Visitor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddTransient<AuthStateHandler>();

//Blazored Storage
builder.Services.AddBlazoredLocalStorage();

//Services and Handlers
builder.Services.AddScoped<GetVisitorHandler>();
builder.Services.AddScoped<GetByIdentificationNumberHandler>();
builder.Services.AddScoped<TokenHandler>();
builder.Services.AddScoped<ScheduledVisitCheckinHandler>();
builder.Services.AddScoped<CreateWalkinHander>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddHttpClient("ApiClient", opts =>
{
    opts.BaseAddress = new Uri("https://localhost:44369/");
});

builder.Services.AddHttpClient("SecureApiClient", opts =>
{
    opts.BaseAddress = new Uri("https://localhost:44369/");
}).AddHttpMessageHandler<AuthStateHandler>();

await builder.Build().RunAsync();
