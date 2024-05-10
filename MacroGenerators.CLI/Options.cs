using CommandLine;

namespace MacroGenerators.CLI
{
    public class Options
    {
        [Option("domain", Required = true, HelpText = "Path to the domain file")]
        public string DomainPath { get; set; } = "";
        [Option("problem", Required = true, HelpText = "Path to the problem file")]
        public string ProblemPath { get; set; } = "";
        [Option("plans", Required = true, HelpText = "Path to the plan files")]
        public IEnumerable<string> Plans { get; set; } = new List<string>();
        [Option("generator", Required = true, HelpText = "What generator to use")]
        public GeneratorOptions Generator { get; set; }

        [Option("amount", Required = false, HelpText = "Max limit on macro generation", Default = -1)]
        public int MaxMacros { get; set; } = -1;
        [Option("out", Required = false, HelpText = "Where to output the macros", Default = "out")]
        public string OutPath { get; set; } = "out";
    }
}
