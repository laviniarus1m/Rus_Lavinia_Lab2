
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Rus_Lavinia_Lab2.Data;

var builder = WebApplication.CreateBuilder(args);

// Conexiunea la baza de date
var connectionString = builder.Configuration.GetConnectionString(
    "Rus_Lavinia_Lab2Context")
    ?? throw new InvalidOperationException(
        "Connection string 'Rus_Lavinia_Lab2Context' not found.");

// Configurarea Razor Pages
builder.Services.AddRazorPages();

// Contextul bazei de date pentru biblioteca
builder.Services.AddDbContext<Rus_Lavinia_Lab2Context>(options =>
    options.UseSqlServer(connectionString));

// Contextul pentru Identity
builder.Services.AddDbContext<LibraryIdentityContext>(options =>
    options.UseSqlServer(connectionString));

// Configurarea Identity pentru Register si Login
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    // Dezactivam temporar confirmarea pentru testarea locala
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<LibraryIdentityContext>();

var app = builder.Build();

// Configurarea aplicatiei
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Autentificare si autorizare
app.UseAuthentication();
app.UseAuthorization();

// Fisiere statice
app.MapStaticAssets();

// Activarea paginilor Razor
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
