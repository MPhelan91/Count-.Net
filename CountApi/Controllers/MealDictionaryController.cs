using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer;
using DatabaseAccessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CountApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class MealDictionaryController : CountControllerBase
  {
    private CountDictionary _dictionary;

    public MealDictionaryController() {
      var dbContext = new DatabaseContext(@"Data Source=.\SQLExpress;User Id=CountUser;Password=Count12345;Initial Catalog=Count_001_DB;");
      _dictionary = new CountDictionary(dbContext);
    }

    // GET: api/MealDictionary
    [HttpGet]
    public IEnumerable<SavedMeal> Get()
    {
      return _dictionary.GetMeals();
    }

    // POST: api/MealDictionary
    [HttpPost]
    public IActionResult Post([FromBody] SavedMeal value)
    {
      return tryCatchServiceCall(() =>_dictionary.AddMeal(value));
    }

    // PUT: api/MealDictionary/5
    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] SavedMeal value)
    {
      value.Id = id;
      return tryCatchServiceCall(() =>_dictionary.EditMeal(value));
    }

    // DELETE: api/ApiWithActions/5
    [HttpDelete("{id}")]
    public IActionResult Delete(int id) {
      return tryCatchServiceCall(() =>_dictionary.RemoveMeal(id));
    }
  }
}
