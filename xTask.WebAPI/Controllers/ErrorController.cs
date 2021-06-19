using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using xTask.WebAPI.Exceptions;

namespace xTask.WebAPI.Controllers
{
    /// <summary>
    /// Generic Error Handler
    /// </summary>
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : Controller
    {
        [Route("/error")]
        public IActionResult Error([FromServices] IWebHostEnvironment webHostEnvironment)
        {
            bool prd = false;
            if (webHostEnvironment != null)
            {
                if (webHostEnvironment.EnvironmentName != "Development")
                {
                    prd = true;
                }
            }
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            if (context.Error is UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            else if (context.Error is CustomBadRequestExceptions)
            {
                return BadRequest(((CustomBadRequestExceptions)context.Error).ModelState); 
            }
            else if (context.Error is InvalidOperationException)
            {
                return BadRequest(context.Error.Message);
            }
            else
            {
                string message = context.Error.Message;
                if (!prd)
                {
                    message = context.Error.ToString();
                }
                return Problem(detail: message, title: "Global Error");
            }
        }

    }
}
