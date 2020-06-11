using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer;
using Common;
using CountApi.Messages;
using DatabaseAccessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CountApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CalorieLogController : CountControllerBase
  {
    private CalorieLog _log;
    public CalorieLogController()
    {
      var dbContext = new DatabaseContext(@"Data Source=.\SQLExpress;User Id=CountUser;Password=Count12345;Initial Catalog=Count_001_DB;");
      _log = new CalorieLog(dbContext);
    }

    [Route("getEntries/{date}")]
    [HttpGet]
    public IEnumerable<CalorieEntry> GetEntries(DateTime date)
    {
      return _log.GetEntries(date);
    }

    [Route("getCounts/{date}")]
    [HttpGet]
    public NutritionalInfo GetCounts(DateTime date)
    {
      return _log.GetCount(date); 
    }

    [Route("calcNutritionalInfo")]
    [HttpPut]
    public IActionResult CalculateNutritionalInfo([FromBody] CalcMessage message)
    {
      return tryCatchServiceCallReturnResult<NutritionalInfo>(() => _log.CalculateNutritionalInfo(message.FoodId, message.Serving)); 
    }

    [Route("copyCalorieEntry/{id}")]
    [HttpPost]
    public IActionResult PostFoodEntry(int id)
    {
      return tryCatchServiceCall(() => _log.CopyEntryToToday(id)); 
    }

    [Route("addFoodEntry")]
    [HttpPost]
    public IActionResult PostFoodEntry([FromBody] FoodPosting newEntry)
    {
      return tryCatchServiceCall(() => _log.AddFoodEntry(newEntry.FoodId, newEntry.Info)); 
    }

    [Route("addManualEntry")]
    [HttpPost]
    public IActionResult PostManualEntry([FromBody] NutritionalInfo info)
    {
      return tryCatchServiceCall(() => _log.AddManualEntry(info)); 
    }

    [Route("addMealEntry")]
    [HttpPost]
    public IActionResult PostMealEntry([FromBody] MealPosting newEntry)
    {
      return tryCatchServiceCall(() => _log.AddMealEntry(newEntry.MealId)); 
    }

    [Route("deleteCalorieEntry/{id}")]
    [HttpDelete]
    public IActionResult DeleteFoodOrManualEntry(int id)
    {
      return tryCatchServiceCall(() => _log.RemoveCalorieEntry(id)); 
    }
  }
}
