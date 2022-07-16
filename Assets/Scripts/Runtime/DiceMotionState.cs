namespace Dev.ComradeVanti.Wurfel
{

    public record DiceMotionState(bool IsMoving, DiceGroundedState GroundedState)
    {

        public bool IsResting => !IsMoving && GroundedState != DiceGroundedState.InAir;
        
        public bool IsRestingFlat => !IsMoving && GroundedState == DiceGroundedState.FlatOnGround;

    }

}