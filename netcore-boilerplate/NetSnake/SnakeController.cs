using System;
using System.Linq;
using AStarNavigator;
using AStarNavigator.Algorithms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using NetSnake.Model;
using NetSnake.Snake;

namespace NetSnake
{
    [Route("")]
    public class SnakeController : Controller
    {
        public GameBoard GameBoard { get; set; }

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
            GameBoard = new GameBoard(request.you, request.board);
            GameBoard.Update(request.you, request.board);

            var head = request.you.body.First();
            var heuristic = new ManhattanHeuristicAlgorithm();
            TileNavigator navigator = new TileNavigator(
                GameBoard,
                GameBoard,
                new PythagorasAlgorithm(),
                heuristic);

            int dist = Int32.MaxValue;
            Tile? foodTile = null;
            foreach (var food in request.board.food)
            {
                var newDist = heuristic.Calculate(ToTile(head), ToTile(food));
                if (newDist< dist && navigator.Navigate(ToTile(head), ToTile(food)).Any() &&
                    GameBoard.GetNeighbors(ToTile(food)).Count(t => !GameBoard.IsBlocked(t)) > 1)
                {
                    dist = (int)newDist;
                    foodTile = ToTile(food);
                }
            }

            if (foodTile.HasValue)
            {
                var startPath = navigator.Navigate(ToTile(head), foodTile.Value).FirstOrDefault();
                if (startPath != null && GetMovement(startPath, head, out var foodMove)) return foodMove;
            }


            // Undvik väggar
            

            var tiles = GameBoard.GetNeighbors(new Tile(head.x, head.y)).Where(x => !GameBoard.IsBlocked(x)).ToList();

            if (tiles.Count == 0)
            {
                return Ok(new Move {Taunt = "Scheisse!!" });
            }

            Random rnd = new Random();
            int index = rnd.Next(0, tiles.Count);

            var tile = tiles[index];

            if (GetMovement(tile, head, out var move)) return move;

            return Ok(new Move());
        }

        private bool GetMovement(Tile tile, Coords head, out IActionResult move)
        {
            switch (tile)
            {
                case Tile t when t.Y < head.y:
                {
                    move = Ok(new Move {Direction = Direction.Up});
                    return true;
                }
                case Tile t when t.Y > head.y:
                {
                    move = Ok(new Move {Direction = Direction.Down});
                    return true;
                }
                case Tile t when t.X < head.x:
                {
                    move = Ok(new Move {Direction = Direction.Left});
                    return true;
                }
                case Tile t when t.X > head.x:
                {
                    move = Ok(new Move {Direction = Direction.Right});
                    return true;
                }
            }

            move = Ok(new Move());
            return false;
        }

        private static Tile ToTile(Coords coords)
        {
            return new Tile(coords.x, coords.y);
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
