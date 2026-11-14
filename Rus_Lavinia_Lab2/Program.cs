
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Rus_Lavinia_Lab2.Data;

var builder = WebApplication.CreateBuilder(args);

// Conexiunea la baza de date
var connectionString = builder.Configuration.GetConnectionString(
    "Rus_Lavinia_Lab2Context")
    ?? throw new InvalidOperationException(
        "Connection string 'Rus_Lavinia_Lab2Context' not found.");

// Punctul 11 - Politica de autorizare Admin
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireRole("Admin"));
});

// Configurarea Razor Pages
builder.Services.AddRazorPages(options =>
{
    // Punctele 3 si 4 - Books
    options.Conventions.AuthorizeFolder("/BookPages");

    options.Conventions.AllowAnonymousToPage("/BookPages/Index");
    options.Conventions.AllowAnonymousToPage("/BookPages/Details");

    // Punctul 11 - Members
    options.Conventions.AuthorizeFolder(
        "/Members", "AdminPolicy");

    // Sarcina finala 1 - Publishers si Categories
    options.Conventions.AuthorizeFolder(
        "/Publishers", "AdminPolicy");

    options.Conventions.AuthorizeFolder(
        "/Categories", "AdminPolicy");
});

// Contextul bazei de date pentru biblioteca
builder.Services.AddDbContext<Rus_Lavinia_Lab2Context>(options =>
    options.UseSqlServer(connectionString));

// Contextul bazei de date pentru Identity
builder.Services.AddDbContext<LibraryIdentityContext>(options =>
    options.UseSqlServer(connectionString));

// Punctul 5 - Identity cu roluri
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<LibraryIdentityContext>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
