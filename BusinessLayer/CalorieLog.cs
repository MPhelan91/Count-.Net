using DatabaseAccessLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer
{
  public class CalorieLog
  {
    private DatabaseContext _context;
    public CalorieLog(DatabaseContext context) {
      _context = context;
    }
    public void AddFoodEntry(int foodId, Serving serving) { 
      //Get food
      //Math.convert(fromServingSize, fromUnit, toServingSize, toUnit, calories, protien)
      //make new class for size and unit together -> Serving
      // make new class for protien and calories together -> nutritional data
    }
    public void AddCustomEntry(int calories, int protien) { }
    public void RemoveFoodEntry(int id) { }
    public void AddMealEntry(int mealId) { }
    public void RemoveMealEntry() { }

    public void GetTodaysCount() { }
  }
}
