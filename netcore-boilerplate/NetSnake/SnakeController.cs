using Microsoft.AspNetCore.Mvc;
using NetSnake.Model;
using NetSnake.Snake;

namespace NetSnake
{
    [Route("")]
    public class SnakeController : Controller
    {
        private GameBoard GameBoard { get; set; }

        [HttpPost]
        [Route("start")]
        public IActionResult Start([FromBody] NetSnake.Model.Request request)
        {
            GameBoard = new GameBoard(request.you, request.board);

            return Ok(new Configuration());
        }

        [HttpPost]
        [Route("move")]
        public IActionResult Move([FromBody] NetSnake.Model.Request request)
        {
            // Hitta närmsta mat
            // Undvik ormar
            GameBoard?.Update(request.board);

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
