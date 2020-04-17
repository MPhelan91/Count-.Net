using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseAccessLayer
{
  public class SavedMeal
  {
    public int Id { get; set; }
    public string MealName { get; set; }
    public int Calories { get; set; }
    public int Protien { get; set; }
  }
}
