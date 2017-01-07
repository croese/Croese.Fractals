using System.Collections.Generic;
using System.Linq;

namespace Croese.Fractals.TurtleGraphics.GraphicsContext
{
    public class SvgContext : IGraphicsContext2D
    {
        private readonly int _height;
        private readonly List<SvgPathSegment> _segments = new List<SvgPathSegment>();
        private readonly int _width;

        public SvgContext(int width = 300, int height = 300)
        {
            _width = width;
            _height = height;
        }

        public int IncreasingXMultiplier => 1;
        public int IncreasingYMultiplier => -1;

        public IGraphicsContext2D DrawLine(double x1, double y1, double x2, double y2, string color)
        {
            _segments.Add(new SvgPathSegment {X1 = x1, Y1 = y1, X2 = x2, Y2 = y2, Color = color});
            return this;
        }

        public IGraphicsContext2D Clear()
        {
            _segments.Clear();
            return this;
        }

        public override string ToString()
        {
            var path = string.Join(string.Empty, _segments.Select(x => x.ToString()));
            var width = _width > 0 ? $"width=\"{_width}\"" : string.Empty;
            var height = _height > 0 ? $"height=\"{_height}\"" : string.Empty;
            return
                $"<svg version=\"1.1\" baseProfile=\"full\" {width} {height} xmlns=\"http://www.w3.org/2000/svg\">{path}</svg>";
        }

        private class SvgPathSegment
        {
            public string Color = "black";
            public double X1;
            public double X2;
            public double Y1;
            public double Y2;

            public override string ToString()
            {
                return $"<line x1=\"{X1}\" y1=\"{Y1}\" x2=\"{X2}\" y2=\"{Y2}\" stroke=\"{Color}\" />";
            }
        }
    }
}