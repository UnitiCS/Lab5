using Bakery.Models;
using Microsoft.EntityFrameworkCore;

namespace Bakery.Data
{
    public partial class BakeryDBContext : DbContext
    {
        public BakeryDBContext(DbContextOptions<BakeryDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BakeryProduct> BakeryProducts { get; set; } = null!;
        public virtual DbSet<BreadRecipe> BreadRecipes { get; set; } = null!;
        public virtual DbSet<Ingredient> Ingredients { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<Supply> Supplies { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;  

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BreadRecipe>()
                .Property(b => b.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Supply>()
                .Property(s => s.Price)
                .HasColumnType("decimal(18,2)");

            // Связь между BreadRecipe и Ingredient
            modelBuilder.Entity<BreadRecipe>()
                .HasOne(br => br.Ingredient)
                .WithMany(i => i.BreadRecipes)
                .HasForeignKey(br => br.IngredientId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь между BreadRecipe и BakeryProduct
            modelBuilder.Entity<BreadRecipe>()
                .HasOne(br => br.BakeryProduct)
                .WithMany(bp => bp.BreadRecipes)
                .HasForeignKey(br => br.BakeryProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь между Order и BakeryProduct
            modelBuilder.Entity<Order>()
                .HasOne(o => o.BakeryProduct)
                .WithMany(bp => bp.Orders)
                .HasForeignKey(o => o.BakeryProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь между Order и Employee
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Employee)
                .WithMany(e => e.Orders)
                .HasForeignKey(o => o.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь между Supply и Ingredient
            modelBuilder.Entity<Supply>()
                .HasOne(s => s.Ingredient)
                .WithMany(i => i.Supplies)
                .HasForeignKey(s => s.IngredientId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);


        }
    }
}