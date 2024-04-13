using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ClothingStore.Models.Entity;
using ClothingStore.Models.Service.product;
using ClothingStore.Models.Service.wishlist;
using ClothingStore.Models.Service.order;
using ClothingStore.Models.Service.orderdetail;
using ClothingStore.Models.Service.vnpay;
using ClothingStore.Models.Service.size;
using ClothingStore.Models.Service.productsize;
using ClothingStore.Models.Service.category;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ClothingStoreDbContext>
    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("ClothingStoreContextConnection")));

builder.Services.AddIdentity<User, IdentityRole>()
    .AddDefaultTokenProviders()
    .AddDefaultUI()
    .AddEntityFrameworkStores<ClothingStoreDbContext>();
builder.Services.AddRazorPages();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Pages/Account/Logout";
    options.LogoutPath = $"/Identity/Pages/Account/AccessDenied";
});

builder.Services.AddControllersWithViews();

// Add services to the container.
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IWishlistService, WishlistService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
builder.Services.AddScoped<ISizeService, SizeService>();
builder.Services.AddScoped<IProductSizeService, ProductSizeService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();



builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddSingleton<IVnPayService, VnPayService>(); // Add Singleton

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

app.UseSession();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();


app.MapControllerRoute(
            name: "Admin",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=RedirectToView}/{id?}"
    );

app.Run();
