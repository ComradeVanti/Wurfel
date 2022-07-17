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
        private ArenaKeeper arenaKeeper;
        private DiceMotionTracker motionTracker;

        private Opt<int> value = Opt.None<int>();
        private bool isMoving;

        public bool IsActivatingEffect { get; private set; }

        public bool IsTouchingSurface { get; private set; }


        public bool IsResting => !isMoving
                                 && IsTouchingSurface
                                 && !IsFrozen;

        public Opt<int> ScoreValue => value.Map(it => it * scoreMultiplier);

        public bool IsFrozen
        {
            get => freezer.IsFrozen;
            set => freezer.IsFrozen = value;
        }


        private void Awake()
        {
            effect = this.TryGetComponent<DiceEffect>();
            freezer = GetComponent<MotionFreezer>();
            arenaKeeper = FindObjectOfType<ArenaKeeper>();
            motionTracker = GetComponent<DiceMotionTracker>();
        }

        private void Update()
        {
            if (transform.position.y < -5)
                arenaKeeper.RemoveDice(this);
        }

        public void OnValueChanged(Opt<int> maybeValue)
        {
            value = maybeValue;
            OnSomethingChanged();
        }

        public void OnIsMovingChanged(bool isMoving)
        {
            this.isMoving = isMoving;
            OnSomethingChanged();
        }

        private void OnSomethingChanged() =>
            value.Iter(value =>
            {
                if (IsResting) TryActivateEffect(value);
            });

        public void OnIsTouchingSurfaceChanged(bool isTouchingSurface)
        {
            IsTouchingSurface = isTouchingSurface;
            OnSomethingChanged();
        }

        public void OnNewRoundStarted() =>
            effect.Iter(it => it.OnRoundStarted());

        public void OnRoundEnded() =>
            effect.Iter(it => it.OnRoundEnded());

        public void OnEffectDone() =>
            IsActivatingEffect = false;

        private void TryActivateEffect(int value) =>
            effect.Iter(it =>
            {
                IsActivatingEffect = true;
                it.Activate(value);
            });

    }

}