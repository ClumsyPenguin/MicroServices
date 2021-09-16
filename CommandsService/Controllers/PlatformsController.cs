using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
         [HttpPost]
        public ActionResult TestInbounConnections()
        {
            #if DEBUG
            Console.WriteLine("--> Inbound POST # Command Service");
            #endif

            return Ok("Inbound test of from platforms controller");
        }
    }
    
}