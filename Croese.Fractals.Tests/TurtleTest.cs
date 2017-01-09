using Croese.Fractals.TurtleGraphics;
using Xunit;

namespace Croese.Fractals.Tests
{
    public class TurtleTest
    {
        [Theory]
        [InlineData(10, 0, -2, 8, 0)]
        [InlineData(10, 90, 2, 10, -2)]
        [InlineData(10, -90, 2, 10, 2)]
        [InlineData(0, 45, 5, 3.5, -3.5)]
        public void TurtleCanMove(int steps1, double turnAmount, int steps2, double finalX, double finalY)
        {
            var t = new Turtle(new TestContext {IncreasingXMultiplier = 1, IncreasingYMultiplier = 1});
            t.Move(steps1).TurnRight(turnAmount).Move(steps2);
            Assert.Equal(finalX, t.CurrentX, 1);
            Assert.Equal(finalY, t.CurrentY, 1);
        }

        [Theory]
        [InlineData(0, 10, 10, 0, "black")]
        [InlineData(90, 10, 0, 10, "red")]
        public void ForwardDoesDrawOnContext(double turnAmount, int steps, double finalX, double finalY, string color)
        {
            var ctx = new TestContext {IncreasingXMultiplier = 1, IncreasingYMultiplier = 1};
            var t = new Turtle(ctx);
            t.SetPenColor(color);
            t.TurnLeft(turnAmount).Forward(steps);
            ctx.AssertDrawLineCalled(0, 0, finalX, finalY, color);
        }

        [Fact]
        public void CanSetTurtlePenColor()
        {
            var t = new Turtle(new TestContext(), penColor: "red");
            Assert.Equal("red", t.PenColor);
            t.SetPenColor("blue");
            Assert.Equal("blue", t.PenColor);
        }

        [Fact]
        public void MoveDoesNotDrawOnContext()
        {
            var ctx = new TestContext {IncreasingXMultiplier = 1, IncreasingYMultiplier = 1};
            var t = new Turtle(ctx);
            t.Move(10);
            ctx.AssertDrawLineNotCalled();
        }

        [Fact]
        public void PushingAndPoppingStateRestoresState()
        {
            var t = new Turtle(new TestContext {IncreasingXMultiplier = 1, IncreasingYMultiplier = 1});
            t.Move(5).TurnRight(90).Move(6);
            Assert.Equal(5, t.CurrentX, 1);
            Assert.Equal(-6, t.CurrentY, 1);
            Assert.Equal(-90, t.Direction360, 1);
            t.PushState();
            t.TurnRight(90).Move(10).TurnRight(90).Move(3);
            Assert.Equal(-5, t.CurrentX, 1);
            Assert.Equal(-3, t.CurrentY, 1);
            Assert.Equal(-270, t.Direction360, 1);
            t.PopState();
            Assert.Equal(5, t.CurrentX, 1);
            Assert.Equal(-6, t.CurrentY, 1);
            Assert.Equal(-90, t.Direction360, 1);
        }

        [Fact]
        public void ScaleAdjustsStepSizeCorrectly()
        {
            var t = new Turtle(new TestContext(), 5);
            t.Scale(10);
            Assert.Equal(50, t.StepSize);
        }

        [Fact]
        public void TurnLeftAdjustsDirection360Correctly()
        {
            var t = new Turtle(new TestContext());
            t.TurnLeft(45).TurnLeft(-10);
            Assert.Equal(35, t.Direction360);
        }

        [Fact]
        public void TurnRightAdjustsDirection360Correctly()
        {
            var t = new Turtle(new TestContext());
            t.TurnRight(45).TurnRight(-10);
            Assert.Equal(-35, t.Direction360);
        }

        [Fact]
        public void TurtleStateFieldsInitializedInConstructor()
        {
            var t = new Turtle(new TestContext(), 10, 5, 15, 60, "red");
            Assert.Equal(10, t.StepSize);
            Assert.Equal(5, t.CurrentX);
            Assert.Equal(15, t.CurrentY);
            Assert.Equal("red", t.PenColor);
            Assert.Equal(60, t.Direction360);
        }
    }
}