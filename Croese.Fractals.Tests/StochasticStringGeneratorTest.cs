using Croese.Fractals.LSystem;
using Xunit;

namespace Croese.Fractals.Tests
{
    public class StochasticStringGeneratorTest
    {
        [Theory]
        [InlineData(0.2, "ABA")]
        [InlineData(0.6, "B")]
        public void ReturnsTheCorrectProduction(double staticRandomNumber, string expected)
        {
            var gen = new StochasticStringGenerator(() => staticRandomNumber);
            gen.AddProduction('A', 0.3, "ABA");
            gen.AddProduction('A', 0.7, "B");
            Assert.Equal(expected, gen.Generate("A", 1));
        }
    }
}