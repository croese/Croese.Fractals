using System;
using Croese.Fractals.TurtleGraphics.GraphicsContext;
using Moq;

namespace Croese.Fractals.Tests
{
    internal class TestContext : IGraphicsContext2D
    {
        private readonly Mock<IGraphicsContext2D> _mock = new Mock<IGraphicsContext2D>();

        public bool Cleared { get; private set; }

        public int IncreasingXMultiplier { get; set; }

        public int IncreasingYMultiplier { get; set; }

        public IGraphicsContext2D DrawLine(double x1, double y1, double x2, double y2, string color)
        {
            return _mock.Object.DrawLine(x1, y1, x2, y2, color);
        }

        public IGraphicsContext2D Clear()
        {
            Cleared = true;
            return this;
        }

        public void AssertDrawLineCalled(double x1, double y1, double x2, double y2, string color)
        {
            _mock.Verify(c => c.DrawLine(It.Is<double>(d => Math.Abs(d - x1) <= 0.1),
                    It.Is<double>(d => Math.Abs(d - y1) <= 0.1),
                    It.Is<double>(d => Math.Abs(d - x2) <= 0.1),
                    It.Is<double>(d => Math.Abs(d - y2) <= 0.1), color),
                Times.Once);
        }

        public void AssertDrawLineNotCalled()
        {
            _mock.Verify(
                c =>
                    c.DrawLine(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(),
                        It.IsAny<string>()), Times.Never);
        }
    }
}