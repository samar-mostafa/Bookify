using Bookify.web.Core.Mapping;
using Bookify.web.Core.Services;
using Bookify.web.Core.Settings;
using Bookify.web.Helpers;
using Bookify.web.Seeds;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using UoN.ExpressiveAnnotations.NetCore.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.Configure<SecurityStampValidatorOptions>(options =>
options.ValidationInterval = TimeSpan.Zero);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>().
    AddDefaultUI().
    AddDefaultTokenProviders();
builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser> ,ApplicationUserClaimsPrincipalFactory>();
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));
builder.Services.AddExpressiveAnnotations();
builder.Services.Configure<CloudinarySettings>(builder.Configuration.
    GetSection(nameof(CloudinarySettings)));
builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
   
    options.Password.RequiredLength = 8;
    options.User.RequireUniqueEmail = true;
  
});
builder.Services.AddTransient<IImageService, ImageService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
var scope= scopeFactory.CreateScope();
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
var userManager=scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
await DefaultRoles.SeedAsync(roleManager);
await DefaultUsers.SeedAdminUserAsync(userManager);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
