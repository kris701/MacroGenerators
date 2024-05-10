using CommandLine;
using CommandLine.Text;
using MacroGenerators.CLI;
using PDDLSharp.CodeGenerators.FastDownward.Plans;
using PDDLSharp.CodeGenerators.PDDL;
using PDDLSharp.ErrorListeners;
using PDDLSharp.Models.FastDownward.Plans;
using PDDLSharp.Models.PDDL;
using PDDLSharp.Models.PDDL.Domain;
using PDDLSharp.Models.PDDL.Problem;
using PDDLSharp.Parsers.FastDownward.Plans;
using PDDLSharp.Parsers.PDDL;
using PDDLSharp.Translators;

namespace MacroGenerators.CLI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var parser = new Parser(with => with.HelpWriter = null);
            var parserResult = parser.ParseArguments<Options>(args);
            parserResult.WithNotParsed(errs => DisplayHelp(parserResult, errs));
            parserResult.WithParsed(Run);
        }

        public static void Run(Options opts)
        {
            opts.DomainPath = RootPath(opts.DomainPath);
            opts.ProblemPath = RootPath(opts.ProblemPath);
            opts.OutPath = RootPath(opts.OutPath);
            if (Directory.Exists(opts.OutPath))
                Directory.Delete(opts.OutPath, true);
            var plansPaths = new List<string>();
            foreach (var path in opts.Plans)
                plansPaths.Add(RootPath(path));
            if (opts.MaxMacros <= 0)
                opts.MaxMacros = int.MaxValue;

            Console.WriteLine("Parsing files...");
            var listener = new ErrorListener();
            var pddlParser = new PDDLParser(listener);
            var domain = pddlParser.ParseAs<DomainDecl>(new FileInfo(opts.DomainPath));
            var problem = pddlParser.ParseAs<ProblemDecl>(new FileInfo(opts.ProblemPath));
            var pddlDecl = new PDDLDecl(domain, problem);
            var plans = new List<ActionPlan>();
            var planParser = new FDPlanParser(listener);
            foreach (var path in plansPaths)
                plans.Add(planParser.ParseAs<ActionPlan>(new FileInfo(path)));

            Console.WriteLine("Building generator...");
            var generator = MacroGeneratorBuilder.GetGenerator(opts.Generator, pddlDecl);

            Console.WriteLine("Finding Macros...");
            var macros = generator.FindMacros(plans, opts.MaxMacros);
            for (int i = 0; i < macros.Count; i++)
                macros[i].Name = $"{macros[i].Name}_{i}";

            Console.WriteLine("Outputting Macros...");
            var codeGenerator = new PDDLCodeGenerator(listener);
            foreach (var macro in macros)
                codeGenerator.Generate(macro, Path.Combine(opts.OutPath, $"{macro.Name}.pddl"));
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
            var sentenceBuilder = SentenceBuilder.Create();
            foreach (var error in errs)
                if (error is not HelpRequestedError)
                    Console.WriteLine(sentenceBuilder.FormatError(error));
        }

        private static void DisplayHelp<T>(ParserResult<T> result, IEnumerable<Error> errs)
        {
            var helpText = HelpText.AutoBuild(result, h =>
            {
                h.AddEnumValuesToHelpText = true;
                return h;
            }, e => e, verbsIndex: true);
            Console.WriteLine(helpText);
            HandleParseError(errs);
        }

        private static string RootPath(string path)
        {
            if (!Path.IsPathRooted(path))
                path = Path.Join(Directory.GetCurrentDirectory(), path);
            path = path.Replace("\\", "/");
            return path;
        }
    }
}