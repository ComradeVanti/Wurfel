using ComradeVanti.CSharpTools;
using UnityEngine;
using UnityEngine.Events;

namespace Dev.ComradeVanti.Wurfel
{

    public abstract class DiceEffect : MonoBehaviour
    {

        [SerializeField] private UnityEvent onExecuted;
        
        private bool wasInAir;
        private Opt<int> lastStrength = Opt.None<int>();


        public bool ExecutedThisRound { get; set; }

        public UnityEvent OnExecuted => onExecuted;
        
        public void OnValueChanged(int strength)
        {
            if (CanExecuteWith(strength))
            {
                ExecutedThisRound = true;
                wasInAir = false;
                lastStrength = Opt.Some(strength);
                ExecuteEffect(strength);
                OnExecuted.Invoke();
            }
        }

        private bool CanExecuteWith(int strength) =>
            !ExecutedThisRound && (wasInAir || !lastStrength.Contains(strength));

        public void OnMotionStateChanged(DiceMotionState motionState)
        {
            if (motionState.GroundedState == DiceGroundedState.InAir)
                wasInAir = true;
        }

        protected abstract void ExecuteEffect(int strength);

    }

}