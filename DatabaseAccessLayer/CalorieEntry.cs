using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseAccessLayer
{
  public class CalorieEntry {
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime EntryDate { get; set; }
    public double Calories { get; set; }
    public double Protien { get; set; }
  }
}
