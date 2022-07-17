namespace Dev.ComradeVanti.Wurfel
{

    public record DiceMotionState(bool IsMoving, DiceGroundedState GroundedState)
    {

        public bool IsTouchingGround => GroundedState != DiceGroundedState.InAir;
        
        public bool IsResting => !IsMoving && IsTouchingGround;
        
        public bool IsRestingFlat => !IsMoving && GroundedState == DiceGroundedState.FlatOnGround;

    }

}