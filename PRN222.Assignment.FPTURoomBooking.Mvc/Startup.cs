using MapsterMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PRN222.Assignment.FPTURoomBooking.Mvc.Hubs;
using PRN222.Assignment.FPTURoomBooking.Repositories.Models;
using PRN222.Assignment.FPTURoomBooking.Repositories.UnitOfWork;
using PRN222.Assignment.FPTURoomBooking.Services.Mappings;
using PRN222.Assignment.FPTURoomBooking.Services.Services;
using PRN222.Assignment.FPTURoomBooking.Services.Services.Interfaces;
using PRN222.Assignment.FPTURoomBooking.Services.Utils.PasswordHasher;

namespace PRN222.Assignment.FPTURoomBooking.Mvc;

public static class Startup
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        // Register Mappings
        builder.Services.AddScoped<IMapper, Mapper>();
        MappingConfig.RegisterMappings();

        // Register Services
        builder.Services.AddScoped<IAccountService, AccountService>();
        builder.Services.AddScoped<IBookingService, BookingService>();
        builder.Services.AddScoped<ICampusService, CampusService>();
        builder.Services.AddScoped<IDepartmentService, DepartmentService>();
        builder.Services.AddScoped<IRoomService, RoomService>();
        builder.Services.AddScoped<ISlotService, SlotService>();

        // Register Utils
        builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
        
        // Register Repositories
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Add SignalR
        builder.Services.AddSignalR();
    }

    public static void ConfigureDatabase(this WebApplicationBuilder builder)
    {
        // Configure Database
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
    }

    public static void ConfigureCookieAuthentication(this WebApplicationBuilder builder)
    {
        // Generate a dynamic cookie name
        var cookieName = $"AuthCookie_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
        
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.Cookie.Name = cookieName;
            });
    }
    
    public static void ConfigureChatHub(this WebApplication app)
    {
        app.MapHub<MessageHub>("/messageHub");
    }
}