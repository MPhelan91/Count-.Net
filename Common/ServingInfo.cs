using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
  public class ServingInfo
  {
    public ServingInfo() { }
    public ServingInfo(double serving, Unit servingUnit) {
      Serving = serving;
      ServingUnit = servingUnit;
    }
    public double Serving { get; set; }
    public Unit ServingUnit { get; set; } 
  }
}
