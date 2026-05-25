using dotnet_rpg.Models;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // адрес будет примерно так: localhost:xxxx/api/character
    [Route("[controller]")]     // адрес будет так: localhost:xxx/character
    public class CharacterController : ControllerBase
    {
        private static Character knight = new Character();

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(knight);
        }
    }
}