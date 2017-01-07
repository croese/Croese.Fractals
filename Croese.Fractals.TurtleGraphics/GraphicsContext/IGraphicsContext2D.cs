namespace Croese.Fractals.TurtleGraphics.GraphicsContext
{
    public interface IGraphicsContext2D
    {
        int IncreasingXMultiplier { get; }
        int IncreasingYMultiplier { get; }
        IGraphicsContext2D DrawLine(double x1, double y1, double x2, double y2, string color);
        IGraphicsContext2D Clear();
    }
}