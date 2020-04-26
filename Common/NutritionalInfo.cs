using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
  public class NutritionalInfo
  {
    public NutritionalInfo(double cal, double protien) {
      Calories = cal;
      Protien = protien;
    }
    public double Calories { get; set; }
    public double Protien { get; set; }
  }
}
