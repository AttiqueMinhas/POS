
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using POS.Data.DataAccess;
using POS.Data.Repositories.Account;
using POS.Data.Repositories.Definition;
using POS.Data.Repositories.Implementation;
using POS.Data.Services.Definition;
using POS.Data.Services.Implementation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<ISQLDataAccess, SQLDataAccess>();
builder.Services.AddScoped<ICategoriesRepository,CategoriesRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ISalesRepository, SalesRepository>();
builder.Services.AddScoped<ITypeDocumentSaleRepository, TypeDocumentSaleRepository>();
builder.Services.AddScoped<ISalesReportRepository, SalesReportRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IMenuService, MenuService>();

builder.Services.AddHttpContextAccessor(); // Add this if you have used IHttpContextAccessor any where make sure to add it before AddSession
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".POS.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(2); // Set session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    //options.Cookie.Expiration = TimeSpan.FromMinutes(2);
});
//For Authorization purpose.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Path to the login page
        options.AccessDeniedPath = "/Account/AccessDenied"; // Path to access denied page
        options.ExpireTimeSpan = TimeSpan.FromMinutes(2);// Match session timeout
        options.SlidingExpiration = false;// Disable sliding expiration for strict timeout
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
        options.Cookie.MaxAge = TimeSpan.FromMinutes(2); // Explicit cookie expiration
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
// Enable session
app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
// Use authentication and authorization middleware
app.UseAuthentication(); // Adds authentication to the request pipeline
app.UseAuthorization();  // Adds authorization to the request pipeline

//app.UseRouting();

//app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    //pattern: "{controller=Home}/{action=Index}/{id?}");
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
