using System;
using System.Collections.Generic;
using System.IO;
using IniParser.Parser;

namespace FractalGen
{
    // [Parameters]
    // Height = 700
    // Width = 700
    // StartX = 0
    // StartY = 350
    // StepSize = 2
    // StartAngle = 0
    // TurnAngle = 60
    // Axiom = 'F'
    // 
    // [Rules]
    // F = 'G-F-G'
    // G = 'F+G+F'

    public class FractalDefinitionParser
    {
        private const string ParametersSectionName = "Parameters";
        private const string RulesSectionName = "Rules";
        private readonly IniDataParser _parser = new IniDataParser();

        public FractalDefinition Parse(TextReader reader)
        {
            var data = _parser.Parse(reader.ReadToEnd());
            var hasParametersSection = data.Sections.ContainsSection(ParametersSectionName);
            if (!hasParametersSection)
                throw new ArgumentException("Must have a 'Parameters' section");

            var hasRulesSection = data.Sections.ContainsSection(RulesSectionName);
            if (!hasRulesSection) throw new ArgumentException("Must have a 'Rules' section");

            var rulesSection = data.Sections[RulesSectionName];
            var emptyRulesSection = rulesSection.Count == 0;
            if (emptyRulesSection)
                throw new ArgumentException("Rules section must be non-empty");

            var parametersSection = data[ParametersSectionName];
            var result = new FractalDefinition
            {
                Height = int.Parse(parametersSection["Height"]),
                Width = int.Parse(parametersSection["Width"]),
                StartX = double.Parse(parametersSection["StartX"]),
                StartY = double.Parse(parametersSection["StartY"]),
                StepSize = int.Parse(parametersSection["StepSize"]),
                StartAngle = double.Parse(parametersSection["StartAngle"]),
                TurnAngle = double.Parse(parametersSection["TurnAngle"]),
                Axiom = parametersSection["Axiom"].Trim('\'')
            };

            foreach (var r in rulesSection)
                result.Rules.Add(r.KeyName[0], r.Value.Trim('\''));

            return result;
        }
    }

    public class FractalDefinition
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public double StartX { get; set; }
        public double StartY { get; set; }
        public int StepSize { get; set; }
        public double StartAngle { get; set; }
        public double TurnAngle { get; set; }
        public string Axiom { get; set; }
        public Dictionary<char, string> Rules { get; set; } = new Dictionary<char, string>();
    }
}