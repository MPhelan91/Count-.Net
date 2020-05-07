using DatabaseAccessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Tests
{
  public static class Common
  {
    public static DatabaseContext CreateInMemoryDatabase(string name, ICollection<SavedFood> foods = null, ICollection<SavedMeal> meals = null, ICollection<FoodEntry> foodEntries = null, ICollection<MealEntry> mealEntries = null) {
      //Create in memory db
      var options = new DbContextOptionsBuilder<DatabaseContext>()
          .UseInMemoryDatabase(databaseName: name)
          .Options;
      var inMemoryDb = new DatabaseContext(options);

      //Add demo data
      if(foods != null) inMemoryDb.SavedFoods.AddRange(foods);
      if(meals != null) inMemoryDb.SavedMeals.AddRange(meals);
      inMemoryDb.SaveChanges();

      if(foodEntries != null) inMemoryDb.FoodEntries.AddRange(foodEntries);
      if(mealEntries != null) inMemoryDb.MealEntries.AddRange(mealEntries);
      inMemoryDb.SaveChanges();

      return inMemoryDb;
    }
  }
}
