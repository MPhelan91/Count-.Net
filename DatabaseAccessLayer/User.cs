using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseAccessLayer
{
  public class User
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int CalorieTarget { get; set; }
    //public int CalorieTargetInterval { get; set; } for changing how often to count calories (daily vs weekly)
    public double CurrentWeight { get; set; }
    public double TargetWeight { get; set; }
    public DateTime WeighInSchedule { get; set; }
  }
}
