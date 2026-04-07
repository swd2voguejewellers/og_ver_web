using Microsoft.EntityFrameworkCore;
using TestSPA.Interfaces;
using TestSPA.Repository;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddScoped<IOGRepository, OGRepository>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
