using DatabaseAccessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Tests
{
  public static class Common
  {
    public static DatabaseContext CreateInMemoryDatabase(string name, ICollection<SavedFood> foods = null, ICollection<SavedMeal> meals = null, ICollection<CalorieEntry> calorieEntries = null) {
      //Create in memory db
      var options = new DbContextOptionsBuilder<DatabaseContext>()
          .UseInMemoryDatabase(databaseName: name)
          .Options;
      var inMemoryDb = new DatabaseContext(options);

      //Clear Any Data already existsing
      inMemoryDb.Database.EnsureDeleted();

      //Add demo data
      if(foods != null) inMemoryDb.SavedFoods.AddRange(foods);
      if(meals != null) inMemoryDb.SavedMeals.AddRange(meals);
      inMemoryDb.SaveChanges();

      if(calorieEntries != null) inMemoryDb.CalorieEntries.AddRange(calorieEntries);
      inMemoryDb.SaveChanges();

      return inMemoryDb;
    }
  }
}
