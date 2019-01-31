using System;
using System.Collections.Generic;
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
            bool foodFound = false;
            sortedFood
            foreach (var food in request.board.food)
            {
                var newDist = heuristic.Calculate(ToTile(head), ToTile(food));
                var pathToFood = navigator.Navigate(ToTile(head), ToTile(food)) ?? new List<Tile>();
                if (newDist < dist && pathToFood.Any() &&
                    GameBoard.GetNeighbors(ToTile(food)).Count(t => !GameBoard.IsBlocked(t)) > 1)
                {
                    dist = (int)newDist;
                    foodTile = ToTile(food);
                    foodFound = true;
                }
            }



            if (foodFound)
            {
                var pathToFood = (navigator.Navigate(ToTile(head), foodTile.Value) ?? new List<Tile>()).ToList();
                var startPath = pathToFood.FirstOrDefault();
                if (pathToFood.Any() && GetMovement(startPath, head, out var foodMove)) return Ok(new Move { Direction = foodMove });
            }
            

            // Undvik väggar
            

            var tiles = GameBoard.GetNeighbors(new Tile(head.x, head.y)).Where(x => !GameBoard.IsBlocked(x)).ToList();

            if (tiles.Count == 0)
            {
                return Ok(new Move {Taunt = "Scheisse!!" });
            }

            var tile = GetBestTile(tiles);


            if (GetMovement(tile, head, out var dir)) return Ok(new Move {Direction = dir});

            return Ok(new Move());
        }

        private Tile GetBestTile(List<Tile> tiles)
        {
            var maxFreeTiles = 0;
            Tile? returnTile = null;

            foreach (var tile in tiles)
            {
                var freeTiles = GameBoard.GetNeighbors(tile).Count(x => !GameBoard.IsBlocked(x));
                if (freeTiles > maxFreeTiles)
                {
                    maxFreeTiles = freeTiles;
                    returnTile = tile;
                }

            }

            if (!returnTile.HasValue)
            {
                Random rnd = new Random();
                int index = rnd.Next(0, tiles.Count);
                returnTile = tiles[index];

            }

            return returnTile.Value;
        }

        private bool GetMovement(Tile tile, Coords head, out Direction dir)
        {
            switch (tile)
            {
                case Tile t when t.Y < head.y:
                {
                    dir = Direction.Up;
                    return true;
                }
                case Tile t when t.Y > head.y:
                {
                    dir = Direction.Down;
                    return true;
                }
                case Tile t when t.X < head.x:
                {
                    dir = Direction.Left;
                    return true;
                }
                case Tile t when t.X > head.x:
                {
                    dir = Direction.Right;
                    return true;
                }
            }

            dir = Direction.Up;
            return false;
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

    public class SortableFood {

    }
}
