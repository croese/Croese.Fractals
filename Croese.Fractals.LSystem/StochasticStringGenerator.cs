using System;
using System.Collections.Generic;
using System.Linq;

namespace Croese.Fractals.LSystem
{
    public class StochasticStringGenerator : LindenmayerStringGenerator
    {
        private readonly Func<double> _randomGenerator;
        private readonly Dictionary<char, List<Production>> _rules = new Dictionary<char, List<Production>>();

        public StochasticStringGenerator(Func<double> randomGenerator)
        {
            _randomGenerator = randomGenerator;
        }

        protected override string Replace(char symbol)
        {
            List<Production> found;
            if (!_rules.TryGetValue(symbol, out found)) return symbol.ToString();

            var randomWeight = _randomGenerator() * found.Sum(p => p.Weight);
            foreach (var production in found)
            {
                randomWeight -= production.Weight;
                if (randomWeight <= 0)
                    return production.Replacement;
            }

            return symbol.ToString();
        }

        public StochasticStringGenerator AddProduction(char symbol, double weight, string replacement)
        {
            List<Production> found;
            var production = new Production
            {
                Weight = weight,
                Replacement = replacement
            };

            if (_rules.TryGetValue(symbol, out found))
                found.Add(production);
            else
                _rules.Add(symbol, new List<Production>
                {
                    production
                });

            return this;
        }

        private class Production
        {
            public double Weight { get; set; }
            public string Replacement { get; set; }
        }
    }
}