using System.Collections.Generic;

namespace Croese.Fractals.LSystem
{
    public class DeterministicStringGenerator : LindenmayerStringGenerator
    {
        private readonly IDictionary<char, string> _replacements;

        public DeterministicStringGenerator(IDictionary<char, string> replacements)
        {
            _replacements = replacements;
        }

        protected override string Replace(char symbol)
        {
            string found;
            return _replacements.TryGetValue(symbol, out found) ? found : symbol.ToString();
        }
    }
}