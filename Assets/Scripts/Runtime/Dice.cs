using System;
using ComradeVanti.CSharpTools;
using ComradeVanti.OptUnity;
using UnityEngine;

namespace Dev.ComradeVanti.Wurfel
{

    public class Dice : MonoBehaviour
    {
        
        private Opt<DiceEffect> effect;
        private MotionFreezer freezer;
        private ArenaKeeper arenaKeeper;
        
        private bool wasInAir;
        private int lastValue;


        public int FaceValue { get; set; }
        
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
            arenaKeeper = FindObjectOfType<ArenaKeeper>();
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
                && effect.Exists(it => it.CanActivate)
                && (wasInAir || valueChanged);

            if (shouldActivateEffect)
                TryActivateEffect();
        }

        public void OnNewRoundStarted() =>
            effect.Iter(it => it.OnRoundStarted());

        public void OnRoundEnded() =>
            effect.Iter(it => it.OnRoundEnded());

        public void OnEffectDone()
        {
            IsActivatingEffect = false;
        }

        private void TryActivateEffect() =>
            effect.Iter(it =>
            {
                IsActivatingEffect = true;
                wasInAir = false;
                lastValue = FaceValue;
                
                it.Activate(FaceValue);
            });

        private void Update()
        {
            if (transform.position.y < -10)
                arenaKeeper.RemoveDice(this);
        }

    }

}