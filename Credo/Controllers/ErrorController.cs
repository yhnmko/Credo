using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Credo.Controllers
{
    public class ErrorController : Controller
    {
        [Route("api/error")]
        public IActionResult LogError()
        {
            //Get hold of the exception that occured
            var exFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exFeature != null)
            {
                //Get the path where the eroor occured
                string path = exFeature.Path;

                //Get the Exception
                Exception ex = exFeature.Error;

                //Log in a flat fire or other storage
                Log.Error(ex, path);

                var error = new { ErrorMessage = ex.Message, ErrorPath = path };

                return BadRequest(error);

            }
            return BadRequest();
        }
    }
}
