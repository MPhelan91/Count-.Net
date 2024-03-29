﻿using System;
using System.Collections.Generic;
using System.Text;
using Common;

namespace DatabaseAccessLayer
{
  public class SavedFood
  {
    public int Id { get; set; }
    public string FoodName { get; set; }
    public int ServingSize { get; set; }
    public Unit ServingUnit { get; set; }
    public int Calories { get; set; }
    public int Protien { get; set; }
    public ServingInfo GetServingInfo() {
      return new ServingInfo(ServingSize, ServingUnit); 
    }
    public NutritionalInfo GetNutritionalInfo() {
      return new NutritionalInfo(Calories, Protien);
    }
  }
}
