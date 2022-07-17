using ComradeVanti.CSharpTools;
using ComradeVanti.OptUnity;
using UnityEngine;

namespace Dev.ComradeVanti.Wurfel
{

    public class Dice : MonoBehaviour
    {

        [SerializeField] private int scoreMultiplier;

        private Opt<DiceEffect> effect;
        private MotionFreezer freezer;

        private bool activatedEffect;
        private bool wasInAir;
        private int lastValue;


        public int FaceValue { get; set; }

        public int ScoreValue => FaceValue * scoreMultiplier;

        public bool IsTouchingGround { get; private set; }

        public bool IsMoving { get; private set; }

        public bool IsActivatingEffect { get; private set; }

        public bool IsFrozen
        {
            get => freezer.IsFrozen;
            set => freezer.IsFrozen = value;
        }


        private void Awake()
        {
            effect = this.TryGetComponent<DiceEffect>();
            freezer = GetComponent<MotionFreezer>();
        }

        public void OnMotionChanged(DiceMotionChange change)
        {
            IsTouchingGround = change.Now(it => it.IsTouchingGround);
            IsMoving = change.Now(it => it.IsMoving);

            if (!IsTouchingGround)
                wasInAir = true;

            var valueChanged = lastValue != FaceValue;

            var shouldActivateEffect =
                change.Started(it => it.IsRestingFlat)
                && !activatedEffect
                && (wasInAir || valueChanged);

            if (shouldActivateEffect)
                TryActivateEffect();
        }

        public void OnNewRoundStarted() =>
            activatedEffect = false;

        public void OnRoundEnded() =>
            activatedEffect = true;

        public void OnEffectDone()
        {
            IsActivatingEffect = false;
        }

        private void TryActivateEffect() =>
            effect.Iter(it =>
            {
                activatedEffect = true;
                IsActivatingEffect = true;
                wasInAir = false;
                lastValue = FaceValue;
                
                it.Activate(FaceValue);
            });

    }

}