using Microsoft.EntityFrameworkCore;
using ProniaApp.Models;
using System.Collections.Generic;

namespace ProniaApp.DAL;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) { }
    public DbSet<Slide> Sliders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }

}
