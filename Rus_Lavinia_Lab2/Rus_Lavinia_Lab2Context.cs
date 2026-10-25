using Microsoft.EntityFrameworkCore;
using Rus_Lavinia_Lab2.Models;

public class Rus_Lavinia_Lab2Context(DbContextOptions<Rus_Lavinia_Lab2Context> options) : DbContext(options)
{
    public DbSet<Rus_Lavinia_Lab2.Models.Book> Book { get; set; } = default!;
    public DbSet<Rus_Lavinia_Lab2.Models.Publisher> Publisher { get; set; } = default!;
    public DbSet<Authors> Authors { get; set; } = default!;
    public DbSet<Category> Category { get; set; } = default!;

    public DbSet<BookCategory> BookCategory { get; set; } = default!;
}
