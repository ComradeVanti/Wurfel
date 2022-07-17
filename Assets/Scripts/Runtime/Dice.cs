using System.Collections;
using ComradeVanti.CSharpTools;
using ComradeVanti.OptUnity;
using UnityEngine;

namespace Dev.ComradeVanti.Wurfel
{

    public class Dice : MonoBehaviour
    {

        private Opt<DiceEffect> effect;
        private DiceFaceTracker faceTracker;
        private DiceMotionTracker motionTracker;
        private MotionFreezer freezer;

        private bool activatedEffect;
        private bool wasInAir;
        private int lastValue;


        public int Value { get; set; }

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
            faceTracker = GetComponent<DiceFaceTracker>();
            motionTracker = GetComponent<DiceMotionTracker>();
            freezer = GetComponent<MotionFreezer>();
        }

        public void OnMotionChanged(DiceMotionChange change)
        {
            IsTouchingGround = change.Now(it => it.IsTouchingGround);
            IsMoving = change.Now(it => it.IsMoving);

            if (!IsTouchingGround)
                wasInAir = true;

            var valueChanged = lastValue != Value;

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
            FindObjectOfType<ArenaKeeper>()
                .IsFrozen = false;
        }

        private void TryActivateEffect() =>
            effect.Iter(it =>
            {
                activatedEffect = true;
                IsActivatingEffect = true;
                wasInAir = false;
                lastValue = Value;

                IEnumerator Test()
                {
                    FindObjectOfType<CameraController>()
                        .Follow(transform);
                    FindObjectOfType<ArenaKeeper>()
                        .IsFrozen = true;
                    yield return new WaitForSeconds(1);
                    it.Activate(Value);
                }

                StartCoroutine(Test());
            });

    }

}