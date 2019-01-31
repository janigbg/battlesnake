using System;
using System.Collections.Generic;
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

            // Undvik väggar
            var head = new Tile(request.you.body.First().x, request.you.body.First().y);
            var body = new Tile(request.you.body.Skip(1).First().x, request.you.body.Skip(1).First().y);

            var tiles = GameBoard.GetNeighbors(head).Where(x => !GameBoard.IsBlocked(x)).ToList();

            if (tiles.Count == 0)
            {
                return Ok(new Move {Taunt = "Scheisse!!" });
            }

            var curDir = GetDirection(body, head);

            var dir = GetBestDirection(tiles, head, curDir);

            return Ok(new Move {Direction = dir});
        }

        private Direction GetDirection(Tile from, Tile to)
        {
            switch (to)
            {
                case Tile t when t.Y < from.Y:
                    return Direction.Up;
                case Tile t when t.Y > from.Y:
                    return Direction.Down;
                case Tile t when t.X < from.X:
                    return Direction.Left;
                case Tile t when t.X > from.X:
                    return Direction.Right;
            }

            // default
            return Direction.Up;
        }

        private Direction GetBestDirection(List<Tile> tiles, Tile head, Direction curDir)
        {
            foreach (var tile in tiles)
            {
                var dir = GetDirection(head, tile);

                if (dir == curDir) return dir;
            }

            Random rnd = new Random();
            int index = rnd.Next(0, tiles.Count);

            var rndTile = tiles[index];

            return GetDirection(head, rndTile);
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
