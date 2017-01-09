using System;
using System.Collections.Generic;
using Croese.Fractals.TurtleGraphics.GraphicsContext;

namespace Croese.Fractals.TurtleGraphics
{
    public class Turtle
    {
        private readonly IGraphicsContext2D _ctx;
        private readonly Stack<TurtleState> _stateStack = new Stack<TurtleState>();

        public Turtle(IGraphicsContext2D ctx, int stepSize = 1, double initialX = 0, double initialY = 0, double initialHeading = 0, string penColor = "black")
        {
            _ctx = ctx;
            PenColor = penColor;
            CurrentX = initialX;
            CurrentY = initialY;
            StepSize = stepSize;
            Direction360 = initialHeading;
        }

        public double CurrentX { get; private set; }
        public double CurrentY { get; private set; }
        public double Direction360 { get; private set; }
        public int StepSize { get; set; }
        public string PenColor { get; private set; }

        public Turtle Forward(int steps = 1)
        {
            var dx = steps * StepSize * Math.Cos(From360(Direction360)) * _ctx.IncreasingXMultiplier;
            var dy = steps * StepSize * Math.Sin(From360(Direction360)) * _ctx.IncreasingYMultiplier;
            _ctx.DrawLine(CurrentX, CurrentY, CurrentX + dx, CurrentY + dy, PenColor);
            CurrentX += dx;
            CurrentY += dy;
            return this;
        }

        public Turtle Move(int steps = 1)
        {
            var dx = steps * StepSize * Math.Cos(From360(Direction360)) * _ctx.IncreasingXMultiplier;
            var dy = steps * StepSize * Math.Sin(From360(Direction360)) * _ctx.IncreasingYMultiplier;
            CurrentX += dx;
            CurrentY += dy;
            return this;
        }

        public Turtle Scale(int size)
        {
            StepSize *= size;
            return this;
        }

        public Turtle TurnLeft(double degrees)
        {
            Direction360 += degrees;
            return this;
        }

        public Turtle TurnRight(double degrees)
        {
            Direction360 -= degrees;
            return this;
        }

        public Turtle PushState()
        {
            _stateStack.Push(new TurtleState {X = CurrentX, Y = CurrentY, Heading = Direction360});
            return this;
        }

        public Turtle PopState()
        {
            var s = _stateStack.Pop();
            CurrentX = s.X;
            CurrentY = s.Y;
            Direction360 = s.Heading;
            return this;
        }

        public Turtle SetPenColor(string color)
        {
            PenColor = color;
            return this;
        }

        private static double From360(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        private class TurtleState
        {
            public double Heading;
            public double X;
            public double Y;
        }
    }
}