using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using PRN222.Assignment.FPTURoomBooking.Blazor;
using PRN222.Assignment.FPTURoomBooking.Blazor.Components;
using PRN222.Assignment.FPTURoomBooking.Blazor.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.ConfigureDatabase();
builder.ConfigureServices();
builder.ConfigureCookieAuthentication();
// Map the Hub
builder.Services.AddSingleton<HubConnection>(sp =>
{
    return new HubConnectionBuilder()
        .WithUrl("https://localhost:5000/messageHub")
        .WithAutomaticReconnect()
        .Build();
});
// Add SignalR
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();





// ... other configurations ...

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.Run();