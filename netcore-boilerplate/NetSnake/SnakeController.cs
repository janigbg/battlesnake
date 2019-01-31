using Microsoft.AspNetCore.Mvc;
using NetSnake.Model;

namespace NetSnake
{
    [Route("")]
    public class SnakeController : Controller
    {
        [HttpPost]
        [Route("start")]
        public IActionResult Start([FromBody] NetSnake.Model.Request request)
        {
            return Ok(new Configuration());
        }

        [HttpPost]
        [Route("move")]
        public IActionResult Move([FromBody] NetSnake.Model.Request request)
        {
            // Hitta närmsta mat
            // Undvik ormar
            // Undvik väggar

            return Ok(new Move());
        }

        [HttpPost]
        [Route("end")]
        public IActionResult End([FromBody] NetSnake.Model.Request request)
        {
            return Ok();
        }

        [HttpPost]
        [Route("ping")]
        public IActionResult Ping()
        {
            return Ok();
        }
    }
}