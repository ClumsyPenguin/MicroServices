using System;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
         [HttpPost]
        public ActionResult TestInboundConnections()
        {
            #if DEBUG
            Console.WriteLine("--> Inbound POST # Command Service");
            #endif

            return Ok("Inbound test of from platforms controller");
        }
    }
    
}