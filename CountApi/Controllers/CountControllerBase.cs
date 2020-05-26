using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CountApi.Controllers
{
  public class CountControllerBase : ControllerBase
  {
    protected IActionResult tryCatchServiceCall(Action serviceCall) {
      try {
        serviceCall.Invoke();
        return Ok();
      }
      catch(Exception ex) {
        return Ok(new ResponseMessage { Status = "failure", FailureMessage = ex.Message });
      }
    }
    protected IActionResult tryCatchServiceCallReturnResult<T>(Func<T> serviceCall) {
      try {
        var result = serviceCall.Invoke();
        return Ok(result);
      }
      catch(Exception ex) {
        return Ok(new ResponseMessage { Status = "failure", FailureMessage = ex.Message });
      }
    }
  }
}
