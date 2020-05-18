using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountApi.Messages
{
  public class CalcMessage
  {
    public int FoodId { get; set; }
    public ServingInfo Serving { get; set; }
  }
}
