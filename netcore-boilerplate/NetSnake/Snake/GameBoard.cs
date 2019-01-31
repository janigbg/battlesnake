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
        private Model.Snake Me { get; }

        public GameBoard(Model.Snake me, Board board)
        {
            X = board.width;
            Y = board.height;
            Me = me;
            Board = new BoardCell[X * Y];
        }

        public void Update(Board board)
        {
            Array.Clear(Board, 0, Board.Length);
            foreach (var food in board.food)
            {
                Board[food.y * X + food.x] = BoardCell.Food;
            }

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
            throw new NotImplementedException();
        }

        public IEnumerable<Tile> GetNeighbors(Tile tile)
        {
            throw new NotImplementedException();
        }
    }
}
