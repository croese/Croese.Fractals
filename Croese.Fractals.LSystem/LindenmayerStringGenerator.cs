using System;
using System.Text.RegularExpressions;

namespace Croese.Fractals.LSystem
{
    public abstract class LindenmayerStringGenerator
    {
        public event Action<string> DebugTrace;

        public string Generate(string initialState, int generationNumber)
        {
            Trace($"n = 0: {initialState}");
            if (generationNumber == 0) return initialState;

            var currentState = initialState;
            for (var i = 1; i <= generationNumber; i++)
            {
                currentState = Regex.Replace(currentState, @".", m => Replace(m.Value[0]));
                Trace($"n = {i}: {currentState}");
            }

            return currentState;
        }

        protected void Trace(string msg)
        {
            DebugTrace?.Invoke(msg);
        }

        protected abstract string Replace(char symbol);
    }
}