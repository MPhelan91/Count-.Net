using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer;
using Common;
using DatabaseAccessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

    [Route("getEntries")]
    [HttpGet]
    public IEnumerable<CalorieEntry> GetEntries()
    {
      return _log.GetCurrentEntries();
    }

    [Route("getCounts")]
    [HttpGet]
    public NutritionalInfo GetCounts()
    {
      return _log.GetCurrentCount(); 
    }

    [Route("addFoodEntry")]
    [HttpPost]
    public IActionResult PostFoodEntry([FromBody] int foodId, [FromBody] NutritionalInfo info)
    {
      return tryCatchServiceCall(() => _log.AddFoodEntry(foodId, info)); 
    }

    [Route("addManualEntry")]
    [HttpPost]
    public IActionResult PostManualEntry([FromBody] NutritionalInfo info)
    {
      return tryCatchServiceCall(() => _log.AddManualEntry(info)); 
    }

    [Route("addManualEntry")]
    [HttpPost]
    public IActionResult PostMealEntry([FromBody] int mealId)
    {
      return tryCatchServiceCall(() => _log.AddMealEntry(mealId)); 
    }

    [Route("deleteFoodOrManualEntry")]
    [HttpDelete("{id}")]
    public IActionResult DeleteFoodOrManualEntry(int id)
    {
      return tryCatchServiceCall(() => _log.RemoveFoodEntry(id)); 
    }

    [Route("deleteMealEntry")]
    [HttpDelete("{id}")]
    public IActionResult DeleteMealEntry(int id)
    {
      return tryCatchServiceCall(() => _log.RemoveFoodEntry(id)); 
    }
  }
}
