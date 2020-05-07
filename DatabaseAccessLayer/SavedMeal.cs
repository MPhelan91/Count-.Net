using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseAccessLayer
{
  public class SavedMeal
  {
    public int Id { get; set; }
    public string MealName { get; set; }
    public double Calories { get; set; }
    public double Protien { get; set; }

    public NutritionalInfo GetNutritionalInfo() {
      return new NutritionalInfo(Calories, Protien);
    }
  }
}
