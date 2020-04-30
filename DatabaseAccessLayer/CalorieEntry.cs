using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseAccessLayer
{
  public class FoodEntry
  {
    public int Id { get; set; }
    public DateTime EntryDate { get; set; }
    public SavedFood FoodForEntry { get; set; }
    public double Calories { get; set; }
    public double Protien { get; set; }
  }

  public class MealEntry
  {
    public int Id { get; set; }
    public DateTime EntryDate { get; set; }
    public SavedMeal MealForEntry { get; set; }
  }
}
