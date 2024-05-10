using PDDLSharp.Models.FastDownward.Plans;

namespace MacroGenerators
{
    /// <summary>
    /// A simple class representing a sequence of grounded actions
    /// </summary>
    public class ActionSequence
    {
        /// <summary>
        /// A sequence of grounded actions
        /// </summary>
        public List<GroundedAction> Actions { get; set; }

        /// <summary>
        /// Main constructor
        /// </summary>
        /// <param name="actions"></param>
        public ActionSequence(List<GroundedAction> actions)
        {
            Actions = actions;
        }

        /// <summary>
        /// Equals override
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            if (obj is ActionSequence op)
                return op.GetHashCode() == GetHashCode();
            return false;
        }

        /// <summary>
        /// The order is important here!
        /// Based on https://stackoverflow.com/a/30758270
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            const int seed = 487;
            const int modifier = 31;
            unchecked
            {
                return Actions.Aggregate(seed, (current, item) =>
                    current * modifier + item.ActionName.GetHashCode());
            }
        }
    }
}
