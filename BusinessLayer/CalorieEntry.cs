using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer
{
  public class CalorieEntry
  {
    public EntryType Type { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }
    public double Calories { get; set; }
    public double Protien { get; set; }
    public DateTime EntryDate { get; set; }
  }
}
