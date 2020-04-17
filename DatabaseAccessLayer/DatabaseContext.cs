using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseAccessLayer
{
  public class DatabaseContext : DbContext 
  {
    private string _connectionString;
    public DatabaseContext(DbContextOptions<DatabaseContext> options): base(options) { }
    public DatabaseContext(string connectionString) {
      _connectionString = connectionString;
    }
    
    public DbSet<MealEntry> MealEntries { get; set; }
    public DbSet<FoodEntry> FoodEntries { get; set; }
    public DbSet<SavedFood> SavedFoods { get; set; }
    public DbSet<SavedMeal> SavedMeals { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<WeighIn> WeighIns { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
      if(!string.IsNullOrWhiteSpace(_connectionString))
        optionsBuilder.UseSqlServer(_connectionString);
    }
  }
}
