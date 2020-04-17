using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseAccessLayer
{
  public class WeighIn
  {
    public int Id { get; set; }
    public DateTime WeighInDate { get; set; }
    public double Weight { get; set; }
  }
}
