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
  public class FoodDictionaryController : CountControllerBase
  {
    private CountDictionary _dictionary;
    public FoodDictionaryController()
    {
      var dbContext = new DatabaseContext(@"Data Source=.\SQLExpress;User Id=CountUser;Password=Count12345;Initial Catalog=Count_001_DB;");
      _dictionary = new CountDictionary(dbContext);
    }
    // GET: api/FoodDictionary
    [HttpGet]
    public IEnumerable<SavedFood> Get()
    {
      return _dictionary.GetFoods();
    }

    // POST: api/FoodDictionary
    [HttpPost]
    public IActionResult Post([FromBody] SavedFood value)
    {
      return tryCatchServiceCall(() =>_dictionary.AddFood(value));
    }

    // PUT: api/FoodDictionary/5
    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] SavedFood value)
    {
      value.Id = id;
      return tryCatchServiceCall(() =>_dictionary.EditFood(value));
    }

    // DELETE: api/ApiWithActions/5
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
      return tryCatchServiceCall(() =>_dictionary.RemoveFood(id));
    }

  }
}
