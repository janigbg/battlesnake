using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AStarNavigator;
using AStarNavigator.Providers;
using NetSnake.Model;

namespace NetSnake.Snake
{
    public enum BoardCell
    {
        Empty = 0,
        Me = 1,
        Snake = 2,
        Food = 3,
    }

    public class GameBoard : IBlockedProvider, INeighborProvider
    {
        private BoardCell[] Board { get; set; }
        private int X { get; }
        private int Y { get; }
        private Model.Snake Me { get; set; }

        public GameBoard(Model.Snake me, Board board)
        {
            X = board.width;
            Y = board.height;
            Me = me;
            Board = new BoardCell[X * Y];
        }

        public void Update(Model.Snake me, Board board)
        {
            Array.Clear(Board, 0, Board.Length);
            foreach (var food in board.food)
            {
                Board[food.y * X + food.x] = BoardCell.Food;
            }

            Me = me;

            foreach (var snake in board.snakes)
            {
                var type = snake.id == Me.id
                    ? BoardCell.Me
                    : BoardCell.Snake;
                foreach (var coords in snake.body)
                {
                    Board[coords.y * X + coords.x] = type;
                }               
            }
        }

        public bool IsBlocked(Tile coord)
        {
            var val = Board[(int) coord.Y * X + (int) coord.Y];
            return val == BoardCell.Snake || val == BoardCell.Me;
        }

        public IEnumerable<Tile> GetNeighbors(Tile tile)
        {
            // Up
            if (tile.Y > 0)
            {
                yield return new Tile(tile.X, tile.Y - 1);
            }
            // down
            if (tile.Y < Y - 1)
            {
                yield return new Tile(tile.X, tile.Y + 1);
            }
            // left
            if (tile.X > 0)
            {
                yield return new Tile(tile.X - 1, tile.Y);
            }
            // right
            if (tile.X < X - 1)
            {
                yield return new Tile(tile.X + 1, tile.Y);
            }
        }
    }
}
