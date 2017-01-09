using System;
using CommandLine;
using CommandLine.Text;
using Croese.Fractals.LSystem;
using Croese.Fractals.TurtleGraphics;
using Croese.Fractals.TurtleGraphics.GraphicsContext;

namespace FractalGen
{
    internal class Options
    {
        [Option('g', HelpText = "The generation to to advance the fractal to.  Must be greater than or equal to 0.",
             DefaultValue = 0, Required = true)]
        public int Generation { get; set; }

        [Option('n', "name", HelpText = "The name of the fractal to generate", Required = true)]
        public string FractalName { get; set; }

        [Option('w', "width", DefaultValue = 700, HelpText = "Canvas width")]
        public int Width { get; set; }

        [Option('h', "height", DefaultValue = 700, HelpText = "Canvas height")]
        public int Height { get; set; }
    }

    internal class Program
    {
        private static int Main(string[] args)
        {
            var options = new Options();
            var usage = HelpText.AutoBuild(options);
            if (Parser.Default.ParseArguments(args, options))
            {
                if (options.Generation < 0)
                    return Error(usage);

                Console.WriteLine(RunFractal(options.Width, options.Height, options.FractalName, options.Generation));
            }
            else
            {
                return Error(usage);
            }

            return 0;
        }

        private static string RunFractal(int width, int height, string name, int generation)
        {
            var ctx = new SvgContext(width, height);

            double initialX;
            double initialY;
            int stepSize;
            double initialAngle;
            double turnAngle;

            string initialState;
            var lsys = new DeterministicStringGenerator();

            switch (name)
            {
                case "koch-curve":
                    initialX = 0;
                    initialY = height / 2D;
                    stepSize = 8;
                    initialAngle = 0;
                    turnAngle = 60;
                    lsys.AddProduction('F', "F+F--F+F");
                    initialState = "F";
                    break;
                case "koch-curve-right":
                    initialX = 0;
                    initialY = height / 2D;
                    stepSize = 8;
                    initialAngle = 0;
                    turnAngle = 90;
                    lsys.AddProduction('F', "F+F-F-F+F");
                    initialState = "F";
                    break;
                case "sierpinski-arrow":
                    initialX = 0;
                    initialY = height;
                    stepSize = 2;
                    initialAngle = 0;
                    turnAngle = 60;
                    lsys.AddProduction('F', "G-F-G");
                    lsys.AddProduction('G', "F+G+F");
                    initialState = "F";
                    break;
                case "sierpinski-tri":
                    initialX = width / 2D;
                    initialY = height / 4D;
                    stepSize = 10;
                    initialAngle = 0;
                    turnAngle = 60;
                    lsys.AddProduction('F', "FF");
                    lsys.AddProduction('X', "--FXF++FXF++FXF--");
                    initialState = "FXF--FF--FF";
                    break;
                case "plant1":
                    initialX = width / 2D;
                    initialY = height;
                    stepSize = 7;
                    initialAngle = 75;
                    turnAngle = 22.5;
                    lsys.AddProduction('0', "F-[[0]+0]+F[+F0]-0");
                    lsys.AddProduction('F', "FF");
                    initialState = "0";
                    break;
                case "plant2":
                    initialX = width / 2D;
                    initialY = height;
                    stepSize = 10;
                    initialAngle = 80;
                    turnAngle = 22.5;
                    lsys.AddProduction('F', "FF-[-F+F+F]+[+F-F-F]");
                    initialState = "F";
                    break;
                case "dragon-curve":
                    initialX = width / 2D;
                    initialY = height / 2D;
                    stepSize = 4;
                    initialAngle = 0;
                    turnAngle = 90;
                    lsys.AddProduction('0', "0+1F");
                    lsys.AddProduction('1', "F0-1");
                    initialState = "F0";
                    break;
                case "hex-gosper-curve":
                    initialX = width / 2D;
                    initialY = height / 2D;
                    stepSize = 6;
                    initialAngle = 60;
                    turnAngle = 60;
                    lsys.AddProduction('F', "F-G--G+F++FF+G-");
                    lsys.AddProduction('G', "+F-GG--G-F++F+G");
                    initialState = "F";
                    break;
                case "peano-curve":
                    initialX = width / 2D;
                    initialY = height / 2D;
                    stepSize = 4;
                    initialAngle = 0;
                    turnAngle = 90;
                    lsys.AddProduction('F', "F+F-F-F-F+F+F+F-F");
                    initialState = "F";
                    break;
                case "quad-koch-island":
                    initialX = width /2D;
                    initialY = height / 2D;
                    stepSize = 3;
                    initialAngle = 0;
                    turnAngle = 90;
                    lsys.AddProduction('F', "F-F+F+FFF-F-F+F");
                    initialState = "F+F+F+F";
                    break;
                case "32-segment-curve":
                    initialX = width / 2D;
                    initialY = height / 2D;
                    stepSize = 2;
                    initialAngle = 0;
                    turnAngle = 90;
                    lsys.AddProduction('F', "-F+F-F-F+F+FF-F+F+FF+F-F-FF+FF-FF+F+F-FF-F-F+FF-F-F+F+F-F+");
                    initialState = "F+F+F+F";
                    break;
                case "sierpinski-arrow-alt":
                    initialX = width / 4D;
                    initialY = height;
                    stepSize = 2;
                    initialAngle = 0;
                    turnAngle = 60;
                    lsys.AddProduction('X', "YF+XF+Y");
                    lsys.AddProduction('Y', "XF-YF-X");
                    initialState = "YF";
                    break;
                default:
                    throw new ArgumentException($"Unknown fractal name: '{name}'");
            }

            var turtle = new Turtle(ctx, stepSize, initialX, initialY, initialAngle);

            var interpreter = new TurtleStringInterpreter(turtle);
            interpreter.AddAction('F', t => t.Forward()).AddAction('G', t => t.Forward());
            interpreter.AddAction('f', t => t.Move()).AddAction('g', t => t.Move());
            interpreter.AddAction('-', t => t.TurnRight(turnAngle));
            interpreter.AddAction('+', t => t.TurnLeft(turnAngle));
            interpreter.AddAction('[', t => t.PushState());
            interpreter.AddAction(']', t => t.PopState());

            var derivation = lsys.Generate(initialState, generation);
            interpreter.Run(derivation);

            return ctx.ToString();
        }

        private static int Error(HelpText usage)
        {
            Console.WriteLine(usage.ToString());
            return 1;
        }
    }
}