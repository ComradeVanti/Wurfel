using System;
using ComradeVanti.CSharpTools;

namespace Dev.ComradeVanti.Wurfel
{

    public record DiceMotionChange(DiceMotionState State, Opt<DiceMotionState> Previous)
    {

        public bool Now(Func<DiceMotionState, bool> pred) =>
            pred(State);

        public bool Before(Func<DiceMotionState, bool> pred) =>
            Previous.Exists(pred);

        public bool Changed(Func<DiceMotionState, bool> pred) =>
            Now(pred) != Before(pred);

        public bool Started(Func<DiceMotionState, bool> pred) =>
            Now(pred) && !Before(pred);

        public bool Stopped(Func<DiceMotionState, bool> pred) =>
            !Now(pred) && Before(pred);

    }

}