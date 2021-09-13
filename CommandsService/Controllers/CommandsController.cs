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
    public class CommandsController : ControllerBase
    {
        public CommandsController()
        {
            
        }

        [HttpPost]
        public ActionResult TestInbounConnections()
        {
            Console.WriteLine("--> Inbound POST # Command Service");

            return Ok("Inbound test of from platforms controller");
        }
    }
}