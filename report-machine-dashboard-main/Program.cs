using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using PacificWeb.Components;
using Radzen;

using Models;
using Services;

//using RealtimeTable.Interfaces;
//using RealtimeTable.Services;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddBlazorBootstrap();
builder.Services.AddRazorPages();


// Add services to the container.
//builder.Services.AddRazorComponents()
//      .AddInteractiveServerComponents().AddHubOptions(options => options.MaximumReceiveMessageSize = 10 * 1024 * 1024);
builder.Services.AddRazorComponents().AddInteractiveServerComponents();



builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" }
        );
}
);

builder.Services.AddAuthorization(config =>
{
    foreach (var userPolicy in UserPolicy.GetPolicies()) // all actions
    {
        config.AddPolicy(userPolicy, cfg => cfg.RequireClaim(userPolicy, "true"));
    }
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "auth_token";
        options.LoginPath = "/login";
        options.Cookie.MaxAge = TimeSpan.FromMinutes(1);
        options.AccessDeniedPath = "/access-denied";
    });
builder.Services.AddCascadingAuthenticationState();


builder.WebHost.UseWebRoot("wwwroot");
builder.WebHost.UseStaticWebAssets();


builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();


// Interface & Service
// Default base url
//builder.Services.AddHttpClient<IApiService, ApiService>(client =>
//{
//    client.BaseAddress = new Uri("https://kopi.padajaya.biz.id/");
//});

//builder.Services.AddScoped<IMachineService, MachineService>();

// Razor Class Library
builder.Services.AddHttpClient<RealtimeTable.Interfaces.IApiService, RealtimeTable.Services.ApiService>(client =>
{
    //client.BaseAddress = new Uri("https://kopi.padajaya.biz.id/");
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]);
});
builder.Services.AddScoped<RealtimeTable.Interfaces.IMachineService, RealtimeTable.Services.MachineService>();
builder.Services.AddScoped<RealtimeTable.Interfaces.IUserService, RealtimeTable.Services.UserService>();
builder.Services.AddScoped<RealtimeTable.Interfaces.ISettingService, RealtimeTable.Services.SettingService>();
builder.Services.AddScoped<RealtimeTable.Interfaces.IPushDataService, RealtimeTable.Services.PushDataService>();

// Add Policy for Admin
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
});









builder.Services.AddScoped<ScreenSizeService>(); // Scoped: New instance per user session

// Register pdjService with a factory method
builder.Services.AddScoped(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("postgresConnection");
//    var context = sp.GetRequiredService<pdjContext>();
    var navigationManager = sp.GetRequiredService<NavigationManager>();
    var dialogService = sp.GetRequiredService<DialogService>(); // Add this

    var jsRuntime = sp.GetRequiredService<IJSRuntime>(); // Add this

    var authProvider = sp.GetRequiredService<AuthenticationStateProvider>();


    return new Services.pdjService(navigationManager, connectionString, dialogService, jsRuntime, authProvider); // Pass dialogService and jsRuntime
});






builder.Services.AddScoped<Services.TabService>(); // Scoped: unique instance per user session (circuit)



builder.Services.AddControllers();
builder.Services.AddRadzenComponents();
builder.Services.AddHttpClient();


var app = builder.Build();
app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();


app.MapBlazorHub(config =>
{
    config.CloseOnAuthenticationExpiration = true;
}).WithOrder(-1);


app.Run();