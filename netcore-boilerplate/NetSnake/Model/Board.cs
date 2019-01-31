using System.Collections.Generic;

namespace NetSnake.Model
{
    public class Board
    {
        public int height { get; set; }
        public int width { get; set; }
        public List<Coords> food { get; set; }
        public List<Snake> snakes { get; set; }
    }
}