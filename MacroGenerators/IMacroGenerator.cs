using PDDLSharp.Models.PDDL;
using PDDLSharp.Models.PDDL.Domain;

namespace MacroGenerators
{
    /// <summary>
    /// Main interface for a generator
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMacroGenerator<T>
    {
        /// <summary>
        /// The PDDL declaration to generate for
        /// </summary>
        public PDDLDecl Declaration { get; }
        /// <summary>
        /// Method to generate a set of macros from a <typeparamref name="T"/>
        /// </summary>
        /// <param name="from"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public List<ActionDecl> FindMacros(T from, int amount = int.MaxValue);
    }
}
