using System;
using System.Linq;
using System.Threading.Tasks;

namespace NetSnake.Model
{
    public class Request
    {
        public Game game { get; set; }
        public int turn { get; set; }
        public Board board { get; set; }
        public Snake you { get; set; }
    }
}
