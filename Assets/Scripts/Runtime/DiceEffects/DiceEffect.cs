using UnityEngine;

namespace Dev.ComradeVanti.Wurfel
{

    public abstract class DiceEffect : MonoBehaviour
    {

        private bool moved;


        public bool ExecutedThisRound { get; set; }

        private bool CanExecute => !ExecutedThisRound && moved;


        public void OnValueChanged(int strength)
        {
            if (CanExecute)
            {
                ExecutedThisRound = true;
                moved = false;
                ExecuteEffect(strength);
            }
        }

        public void OnMotionStateChanged(DiceMotionState motionState)
        {
            if (motionState.GroundedState != DiceGroundedState.FlatOnGround)
                moved = true;
        }
        
        protected abstract void ExecuteEffect(int strength);

    }

}