using Microsoft.EntityFrameworkCore;
using EcommerceTelysia.Models;
using Microsoft.AspNetCore.Http.Features;
using EcommerceTelysia.Models;
using EcommerceTelysia.@class;

var builder = WebApplication.CreateBuilder(args);



// ✅ Add Razor Pages with Runtime Compilation
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddScoped<AddToCartItems>();
builder.Services.AddScoped<PhonePePaymentService>();

// ✅ Configure Session with Idle Timeout
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(40);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ✅ Configure Entity Framework with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Telysia"))
);

// ✅ Register IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// ✅ Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

// ✅ Map API and Razor Pages
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();  // Ensures API controllers work
    endpoints.MapRazorPages();   // Ensures Razor Pages work
});

// ✅ Run App
app.Run();
