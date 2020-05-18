using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountApi.Messages
{
  public class FoodPosting
  {
    public int FoodId { get; set; }
    public NutritionalInfo Info { get; set; }
  }
}
