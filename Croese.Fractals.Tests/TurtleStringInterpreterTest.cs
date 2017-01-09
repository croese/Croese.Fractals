using Croese.Fractals.TurtleGraphics;
using Xunit;

namespace Croese.Fractals.Tests
{
    public class TurtleStringInterpreterTest
    {
        [Fact]
        public void RunExecutesAction()
        {
            var ctx = new TestContext {IncreasingYMultiplier = 1, IncreasingXMultiplier = 1};
            var t = new Turtle(ctx);
            var interpreter = new TurtleStringInterpreter(t);
            interpreter.AddAction('A', turtle => turtle.Move(10));
            Assert.Equal(0, t.CurrentX);
            interpreter.Run("A");
            Assert.Equal(10, t.CurrentX);
        }

        [Fact]
        public void RunExecutesMultipleActions() {
            var ctx = new TestContext { IncreasingYMultiplier = 1, IncreasingXMultiplier = 1 };
            var t = new Turtle(ctx);
            var interpreter = new TurtleStringInterpreter(t);
            interpreter.AddAction('A', turtle => turtle.Move(10));
            interpreter.AddAction('B', turtle => turtle.Move(5));
            Assert.Equal(0, t.CurrentX);
            interpreter.Run("AB");
            Assert.Equal(15, t.CurrentX);
        }

        [Fact]
        public void RunSkipsUnknownSymbols() {
            var ctx = new TestContext { IncreasingYMultiplier = 1, IncreasingXMultiplier = 1 };
            var t = new Turtle(ctx);
            var interpreter = new TurtleStringInterpreter(t);
            interpreter.AddAction('A', turtle => turtle.Move(10));
            Assert.Equal(0, t.CurrentX);
            interpreter.Run("X");
            Assert.Equal(0, t.CurrentX);
        }
    }
}