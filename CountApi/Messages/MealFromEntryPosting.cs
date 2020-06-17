using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountApi.Messages
{
  public class MealFromEntryPosting
  {
    public string MealName { get; set; }
    public int[] EntryIds { get; set; }
  }
}
