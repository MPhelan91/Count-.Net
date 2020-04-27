using DatabaseAccessLayer;
using System;
using System.Collections.Generic;
using System.Text;
using Common;
using MathLibrary;

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

    public void AddFoodEntry() { }
    public void AddManualEntry(int calories, int protien) { }
    public void RemoveFoodEntry(int foodId) { }
    public void AddMealEntry(int mealId) { }
    public void RemoveMealEntry(int mealId) { }

    public void GetTodaysCount() { }
  }
}
