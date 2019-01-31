using System.Linq;
using AStarNavigator;
using Microsoft.AspNetCore.Mvc;
using NetSnake.Model;
using NetSnake.Snake;

namespace NetSnake
{
    [Route("")]
    public class SnakeController : Controller
    {
        public static GameBoard GameBoard { get; set; }

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
            GameBoard.Update(request.you, request.board);

            // Undvik väggar
            var head = request.you.body.First();

            var tiles = GameBoard.GetNeighbors(new Tile(head.x, head.y));

            var tile = tiles.FirstOrDefault(x => !GameBoard.IsBlocked(x));

            switch (tile)
            {
                case Tile t when t.Y < head.y:
                    return Ok(new Move { Direction = Direction.Up });
                case Tile t when t.Y > head.y:
                    return Ok(new Move { Direction = Direction.Down });
                case Tile t when t.X < head.x:
                    return Ok(new Move { Direction = Direction.Left });
                case Tile t when t.X > head.x:
                    return Ok(new Move { Direction = Direction.Right });
            }

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
