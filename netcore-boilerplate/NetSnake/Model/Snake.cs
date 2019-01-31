﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetSnake.Model
{
    public class Snake
    {
        public string id { get; set; }
        public string name { get; set; }
        public int health { get; set; }
        public List<Coords> body { get; set; }
    }
}