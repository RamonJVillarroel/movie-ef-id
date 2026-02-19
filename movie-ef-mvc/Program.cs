using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using movie_ef_mvc.Data;
using movie_ef_mvc.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Incluir dbcontext
builder.Services.AddDbContext<MovieDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//inplementa identity
builder.Services.AddIdentityCore<Usuario>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 3;
    options.Password.RequireUppercase = false;
}
)
    .AddRoles<IdentityRole>()// se tiene que agregar a mano cuando se usa el solo el core de identity
    .AddEntityFrameworkStores<MovieDbContext>()
    .AddSignInManager();// se tiene que agregar a mano cuando se usa el solo el core de identity
// Cookies se tiene que agregar a mano cuando se usa el solo el core de identity
//Manejo de la cookie. Lo ponemos en default, pero hay que ponerlo.
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultScheme = IdentityConstants.ApplicationScheme;
})
.AddIdentityCookies();

builder.Services.ConfigureApplicationCookie(o =>
{
    o.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    o.SlidingExpiration = true;
    o.LoginPath = "/Usuario/Login";
    o.AccessDeniedPath = "/Usuario/AccessDenied";
});

var app = builder.Build();
// invocar la ejecucion del dbseader con using scope 
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<MovieDbContext>();
        //var userManager = services.GetRequiredService<UserManager<Usuario>>();
        //var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await DbSeeder.Seed(context);
    }
    catch (Exception ex)
    {
        // Log errors or handle them as needed
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
