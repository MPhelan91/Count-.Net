using DatabaseAccessLayer;
using System;
using System.Collections.Generic;
using System.Text;
using Common;
using MathLibrary;
using System.Linq;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq.Expressions;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer
{
  public class CalorieLog
  {
    private DatabaseContext _context;
    private CountDictionary _countDictionary;
    public CalorieLog(DatabaseContext context, DateTime? startDate = null, DateTime? endDate = null) {
      _context = context;
      _countDictionary = new CountDictionary(context);
    }
    public NutritionalInfo CalculateNutritionalInfo(int foodId, ServingInfo portion) {
      var food = _countDictionary.getFood(foodId);
      return Conversions.Convert(food.GetServingInfo(), food.GetNutritionalInfo(), portion);
    }

    public void ValidateEntry(NutritionalInfo info) {
      if(info.Calories == 0) {
        throw new ArgumentException("Calorie for entry must be greater than 0");
      }
    }

    public void AddFoodEntry(int foodId, NutritionalInfo info) {
      ValidateEntry(info);
      var food = _countDictionary.getFood(foodId);
      var newEntry = new CalorieEntry { Name = food.FoodName, Calories = info.Calories, Protien = info.Protien, EntryDate = DateTime.Now };
      _context.CalorieEntries.Add(newEntry);
      _context.SaveChanges();
    }

    public void AddManualEntry(NutritionalInfo info) { 
      ValidateEntry(info);
      var newEntry = new CalorieEntry { Name="Manual Entry", Calories = info.Calories, Protien = info.Protien, EntryDate = DateTime.Now  };
      _context.CalorieEntries.Add(newEntry);
      _context.SaveChanges();
    }
    public void AddMealEntry(int mealId) { 
      var meal = _countDictionary.getMeal(mealId);
      var newEntry = new CalorieEntry { Name=meal.MealName, Calories= meal.Calories, Protien=meal.Protien, EntryDate = DateTime.Now };
      _context.CalorieEntries.Add(newEntry);
      _context.SaveChanges();
    }

    public void RemoveCalorieEntry(int entryId) {
      var existingEntry = getCalorieEntry(entryId);
      _context.CalorieEntries.Remove(existingEntry);
      _context.SaveChanges();
    }

    private CalorieEntry getCalorieEntry(int entryId) {
      var existingEntry = _context.CalorieEntries.SingleOrDefault(o => o.Id == entryId);
      if(existingEntry == null) {
        throw new ArgumentException(string.Format("No CalorieEntry exists with id {0}", entryId));
      }
      return existingEntry;
    }

    public void CopyEntryToToday(int entryId) {
      var existingEntry = getCalorieEntry(entryId);
      var newEntry = new CalorieEntry {
        Name = existingEntry.Name,
        Calories = existingEntry.Calories,
        Protien = existingEntry.Protien,
        EntryDate = DateTime.Now
      };
      _context.CalorieEntries.Add(newEntry);
      _context.SaveChanges();
    }

    public NutritionalInfo GetCount(DateTime startDate) {
      var endDate = startDate + TimeSpan.FromDays(1);

      var entryQuery = from entry in _context.CalorieEntries
                       where entry.EntryDate >= startDate
                       where entry.EntryDate < endDate
                       group entry by 1 into g
                       select new NutritionalInfo(g.Sum(o => o.Calories), g.Sum(o => o.Protien));

      return entryQuery.FirstOrDefault() ?? new NutritionalInfo(0,0);
    }

    public CalorieEntry[] GetEntries(DateTime startDate) {
      var endDate = startDate + TimeSpan.FromDays(1);

      var entryQuery = from entry in _context.CalorieEntries
                       where entry.EntryDate >= startDate
                       where entry.EntryDate < endDate
                       select entry;

      return entryQuery.OrderByDescending(o => o.EntryDate).ToArray();
    }
  }
}
