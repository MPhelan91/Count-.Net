using DatabaseAccessLayer;
using System;
using System.Collections.Generic;
using System.Text;
using Common;
using MathLibrary;
using System.Linq;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Internal;

namespace BusinessLayer
{
  public class CalorieLog
  {
    private DatabaseContext _context;
    private CountDictionary _countDictionary;
    public CalorieLog(DatabaseContext context) {
      _context = context;
      _countDictionary = new CountDictionary(context);

    }
    public NutritionalInfo CalculateNutritional(int foodId, ServingInfo portion) {
      var food = _countDictionary.getFood(foodId);
      return Conversions.Convert(food.GetServingInfo(), food.GetNutritionalInfo(), portion);
    }

    private void validateEntry(NutritionalInfo info) {
      if(info.Calories == 0) {
        throw new ArgumentException("Calorie for entry must be greater than 0");
      }
    }

    public void AddFoodEntry(int foodId, NutritionalInfo info) {
      validateEntry(info);
      var food = _countDictionary.getFood(foodId);
      var newEntry = new FoodEntry { FoodForEntry = food, Calories = info.Calories, Protien = info.Protien, EntryDate = DateTime.Today };
      _context.FoodEntries.Add(newEntry);
      _context.SaveChanges();
    }
    public void AddManualEntry(NutritionalInfo info) { 
      validateEntry(info);
      var newEntry = new FoodEntry { Calories = info.Calories, Protien = info.Protien, EntryDate = DateTime.Today  };
      _context.FoodEntries.Add(newEntry);
      _context.SaveChanges();
    }
    public void AddMealEntry(int mealId) { 
      var meal = _countDictionary.getMeal(mealId);
      var newEntry = new MealEntry { MealForEntry=meal, EntryDate = DateTime.Today };
      _context.MealEntries.Add(newEntry);
      _context.SaveChanges();
    }
    public void RemoveFoodEntry(int foodEntryId) { 
      var existingEntry = _context.FoodEntries.SingleOrDefault(o => o.Id == foodEntryId);
      if(existingEntry == null) {
        throw new ArgumentException(string.Format("No FoodEntry exists with id {0}", foodEntryId);
      }
      _context.FoodEntries.Remove(existingEntry);
      _context.SaveChanges();
    }
    public void RemoveMealEntry(int mealEntryId) {
      var existingEntry = _context.MealEntries.SingleOrDefault(o => o.Id == mealEntryId);
      if(existingEntry == null) {
        throw new ArgumentException(string.Format("No MealEntry exists with id {0}", mealEntryId);
      }
      _context.MealEntries.Remove(existingEntry);
      _context.SaveChanges();
    }

    /*Think about total vs calories per entry matching up and rounding*/

    public int GetCurrentCount() {
      //Linq query to sum calories
      return 0;
    }

    public IList<CalorieEntry> GetCurrentEntries() {
      //Linq quey to get entries
      return new List<CalorieEntry>();
    }
  }
}
