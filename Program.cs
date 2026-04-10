using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using TestSPA.Interfaces;
using TestSPA.Repository;

var builder = WebApplication.CreateBuilder(args);
var sessionTimeoutMinutes = builder.Configuration.GetValue<int?>("Authentication:SessionTimeoutMinutes") ?? 20;

//add db context
 builder.Services.AddDbContext<AppDbContext>(options  =>
 {
     options.UseSqlServer(builder.Configuration.GetConnectionString("VogueDBCM"));
 });

builder.Services.AddDbContext<AppDbContext2>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("VogueDBNCM"));
});

builder.Services.AddDbContext<AppDbContext1>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DataCommon"));
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IOGRepository, OGRepository>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";
        options.Cookie.HttpOnly = true;
        options.Cookie.Name = "TestSPA.Auth";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(sessionTimeoutMinutes);
        options.SlidingExpiration = true;
        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = context =>
            {
                if (IsAjaxRequest(context.Request))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                }

                context.Response.Redirect(context.RedirectUri);
                return Task.CompletedTask;
            },
            OnRedirectToAccessDenied = context =>
            {
                if (IsAjaxRequest(context.Request))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return Task.CompletedTask;
                }

                context.Response.Redirect(context.RedirectUri);
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

static bool IsAjaxRequest(HttpRequest request)
{
    if (request.Headers.TryGetValue("X-Requested-With", out var requestedWith) &&
        string.Equals(requestedWith, "XMLHttpRequest", StringComparison.OrdinalIgnoreCase))
    {
        return true;
    }

    if (request.Headers.TryGetValue("Accept", out var acceptHeader) &&
        acceptHeader.Any(value => value?.Contains("application/json", StringComparison.OrdinalIgnoreCase) == true))
    {
        return true;
    }

    return false;
}
