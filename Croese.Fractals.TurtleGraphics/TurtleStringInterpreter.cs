using System;
using System.Collections.Generic;

namespace Croese.Fractals.TurtleGraphics
{
    public class TurtleStringInterpreter
    {
        private readonly Dictionary<char, Action<Turtle>> _actions = new Dictionary<char, Action<Turtle>>();
        private readonly Turtle _turtle;

        public TurtleStringInterpreter(Turtle turtle)
        {
            _turtle = turtle;
        }

        public TurtleStringInterpreter AddAction(char symbol, Action<Turtle> action)
        {
            _actions[symbol] = action;
            return this;
        }

        public void Run(string input)
        {
            Action<Turtle> found;
            foreach (var c in input)
                if (_actions.TryGetValue(c, out found))
                {
                    found(_turtle);
                }
        }
    }
}