using System.Collections.Generic;

namespace Croese.Fractals.LSystem
{
    public class DeterministicStringGenerator : LindenmayerStringGenerator
    {
        private readonly IDictionary<char, string> _replacements;

        public DeterministicStringGenerator()
        {
            _replacements = new Dictionary<char, string>();
        }

        public DeterministicStringGenerator(IDictionary<char, string> replacements)
        {
            _replacements = replacements;
        }

        protected override string Replace(char symbol)
        {
            string found;
            return _replacements.TryGetValue(symbol, out found) ? found : symbol.ToString();
        }

        public DeterministicStringGenerator AddProduction(char symbol, string replacement)
        {
            _replacements[symbol] = replacement;
            return this;
        }
    }
}