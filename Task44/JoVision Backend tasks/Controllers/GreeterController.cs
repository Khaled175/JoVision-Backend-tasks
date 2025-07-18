﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JoVision_Backend_tasks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GreeterController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get([FromQuery] string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return "Hello anonymous";
            }
            else
            {
                return $"Hello, {name}";
            }
        }
    }
}
