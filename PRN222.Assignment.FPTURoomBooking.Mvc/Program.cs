using PRN222.Assignment.FPTURoomBooking.Mvc;
using Microsoft.AspNetCore.Cors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.ConfigureDatabase();
builder.ConfigureServices();
builder.ConfigureCookieAuthentication();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorApp",
        builder =>
        {
            builder
                .WithOrigins("https://localhost:7000") // Your Blazor app URL
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials(); // Important for SignalR
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowBlazorApp");

app.UseAuthorization();

app.ConfigureChatHub();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();