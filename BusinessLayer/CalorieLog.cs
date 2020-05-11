﻿using DatabaseAccessLayer;
using System;
using System.Collections.Generic;
using System.Text;
using Common;
using MathLibrary;
using System.Linq;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq.Expressions;

namespace BusinessLayer
{
  public class CalorieLog
  {
    private DatabaseContext _context;
    private CountDictionary _countDictionary;
    private DateTime _startDate;
    private DateTime _endDate;
    public CalorieLog(DatabaseContext context, DateTime? startDate = null, DateTime? endDate = null) {
      _context = context;
      _countDictionary = new CountDictionary(context);
      _startDate = startDate ?? DateTime.Today;
      _endDate = endDate ?? DateTime.Today + TimeSpan.FromDays(1);

      if(startDate >= _endDate) {
        throw new ArgumentException("The calorie log's start date must be before the end date");
      }

    }
    public NutritionalInfo CalculateNutritional(int foodId, ServingInfo portion) {
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
      var newEntry = new FoodEntry { FoodForEntry = food, Calories = info.Calories, Protien = info.Protien, EntryDate = DateTime.Now };
      _context.FoodEntries.Add(newEntry);
      _context.SaveChanges();
    }
    public void AddManualEntry(NutritionalInfo info) { 
      ValidateEntry(info);
      var newEntry = new FoodEntry { Calories = info.Calories, Protien = info.Protien, EntryDate = DateTime.Now  };
      _context.FoodEntries.Add(newEntry);
      _context.SaveChanges();
    }
    public void AddMealEntry(int mealId) { 
      var meal = _countDictionary.getMeal(mealId);
      var newEntry = new MealEntry { MealForEntry=meal, EntryDate = DateTime.Now };
      _context.MealEntries.Add(newEntry);
      _context.SaveChanges();
    }
    public void RemoveFoodEntry(int foodEntryId) { 
      var existingEntry = _context.FoodEntries.SingleOrDefault(o => o.Id == foodEntryId);
      if(existingEntry == null) {
        throw new ArgumentException(string.Format("No FoodEntry exists with id {0}", foodEntryId));
      }
      _context.FoodEntries.Remove(existingEntry);
      _context.SaveChanges();
    }
    public void RemoveMealEntry(int mealEntryId) {
      var existingEntry = _context.MealEntries.SingleOrDefault(o => o.Id == mealEntryId);
      if(existingEntry == null) {
        throw new ArgumentException(string.Format("No MealEntry exists with id {0}", mealEntryId));
      }
      _context.MealEntries.Remove(existingEntry);
      _context.SaveChanges();
    }


    public NutritionalInfo GetCurrentCount() {
      var foodQuery = from entry in _context.FoodEntries
                      where entry.EntryDate >= _startDate
                      where entry.EntryDate < _endDate
                      group entry by 1 into g
                      select new {
                        Calories = g.Sum(o => o.Calories),
                        Protien = g.Sum(o => o.Protien)
                      };

      var mealQuery = from entry in _context.MealEntries
                      where entry.EntryDate >= _startDate
                      where entry.EntryDate < _endDate
                      join meal in _context.SavedMeals on entry.MealForEntry equals meal 
                      group meal by 1 into g
                      select new {
                        Calories = g.Sum(o => o.Calories),
                        Protien = g.Sum(o => o.Protien)
                      };

      var finalQuery = from f in foodQuery.Union(mealQuery)
                       group f by 1 into g
                       select new NutritionalInfo(g.Sum(o => o.Calories), g.Sum(o => o.Protien));

      return finalQuery.First();
    }

    public CalorieEntry[] GetCurrentEntries() {
      var foodQuery = from entry in _context.FoodEntries
                      where entry.EntryDate >= _startDate
                      where entry.EntryDate < _endDate
                      join food in _context.SavedFoods on entry.FoodForEntry equals food into entryXfood
                      from food in entryXfood.DefaultIfEmpty()
                      select new CalorieEntry {
                        Name =  food != null ? food.FoodName : "Manual Entry",
                        Calories = entry.Calories,
                        Protien = entry.Protien,
                        EntryDate = entry.EntryDate
                      };

      var mealQuery = from entry in _context.MealEntries
                      where entry.EntryDate >= _startDate
                      where entry.EntryDate < _endDate
                      join meal in _context.SavedMeals on entry.MealForEntry equals meal
                      select new CalorieEntry{
                        Name = meal.MealName,
                        Calories = meal.Calories,
                        Protien =  meal.Protien,
                        EntryDate = entry.EntryDate
                      };

      return foodQuery.Union(mealQuery).OrderByDescending(o => o.EntryDate).ToArray();
    }
  }
}
