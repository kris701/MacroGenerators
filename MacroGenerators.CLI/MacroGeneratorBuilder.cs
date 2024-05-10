using PDDLSharp.Models.FastDownward.Plans;
using PDDLSharp.Models.PDDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroGenerators.CLI
{
    public enum GeneratorOptions
    {
        Sequential
    }
    public static class MacroGeneratorBuilder
    {
        private static Dictionary<GeneratorOptions, Func<PDDLDecl,IMacroGenerator<List<ActionPlan>>>> _generators = new Dictionary<GeneratorOptions, Func<PDDLDecl, IMacroGenerator<List<ActionPlan>>>>()
        {
            { GeneratorOptions.Sequential, (d) => new SequentialMacroGenerator(d) }
        };

        public static IMacroGenerator<List<ActionPlan>> GetGenerator(GeneratorOptions option, PDDLDecl decl) => _generators[option](decl);
    }
}
