using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using VisitorManagementSystem.SelfServiceUI;
using VisitorManagementSystem.SelfServiceUI.Handlers.Visitor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//Services and Handlers
builder.Services.AddScoped<GetVisitorHandler>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddHttpClient("ApiClient", opts =>
{
    opts.BaseAddress = new Uri("http://localhost:5081");
});

await builder.Build().RunAsync();
