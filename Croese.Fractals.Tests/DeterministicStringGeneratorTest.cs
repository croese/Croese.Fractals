using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Croese.Fractals.LSystem;
using Xunit;

namespace Croese.Fractals.Tests
{
    public class DeterministicStringGeneratorTest
    {
        [Theory]
        [InlineData(0, "A", "A -> 'AB', B -> 'A'", "A")]
        [InlineData(1, "A", "A -> 'AB', B -> 'A'", "AB")]
        [InlineData(7, "A", "A -> 'AB', B -> 'A'", "ABAABABAABAABABAABABAABAABABAABAAB")]
        [InlineData(0, "0", "1 -> '11', 0 -> '1[0]0'", "0")]
        [InlineData(1, "0", "1 -> '11', 0 -> '1[0]0'", "1[0]0")]
        [InlineData(3, "0", "1 -> '11', 0 -> '1[0]0'", "1111[11[1[0]0]1[0]0]11[1[0]0]1[0]0")]
        [InlineData(1, "X", "A -> 'AB'", "X")] // implicit identity rule if no rule is found for a given symbol
        public void GenerateReturnsCorrectStateForGeneration(int n, string axiom, string rules, string expected)
        {
            var r = ParseRules(rules);
            var gen = new DeterministicStringGenerator(r);
            Assert.Equal(expected, gen.Generate(axiom, n));
        }

        private Dictionary<char, string> ParseRules(string rules)
        {
            return Regex.Split(rules, @",\s*").Select(s =>
            {
                var parts = Regex.Split(s, @"\s*->\s*").Select(x => x.Trim()).ToArray();
                return Tuple.Create(parts[0][0], parts[1].Trim('\''));
            }).ToDictionary(p => p.Item1, p => p.Item2);
        }
    }
}