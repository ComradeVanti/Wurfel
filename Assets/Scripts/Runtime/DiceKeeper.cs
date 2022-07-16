using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ComradeVanti.CSharpTools;
using ComradeVanti.OptUnity;
using UnityEngine;
using UnityEngine.Events;

namespace Dev.ComradeVanti.Wurfel
{

    public class DiceKeeper : MonoBehaviour
    {

        [SerializeField] private CameraController cameraController;
        [SerializeField] private UnityEvent onAllDieBecameStill;

        private readonly List<Dice> die = new List<Dice>();


        public void OnDiceLaunched() =>
            AllowEffects();

        private void AllowEffects() =>
            die.Iter(it => it.AllowEffects());

        private void PreventEffects() =>
            die.Iter(it => it.PreventEffects());

        public void OnDiceSpawned(GameObject diceGameObject)
        {
            var dice = new Dice(diceGameObject);
            die.Add(dice);
            dice.OnEffectExecuted.Iter(it => it.AddListener(() => cameraController.Follow(diceGameObject.transform)));
            StartCoroutine(WaitForDieToStopMoving());
        }

        private IEnumerator WaitForDieToStopMoving()
        {
            while (!die.All(it => it.IsResting))
                yield return new WaitForSeconds(1f);
            yield return new WaitForSeconds(1f);
            PreventEffects();
            onAllDieBecameStill.Invoke();
        }


        private class Dice
        {

            private readonly Opt<DiceEffect> effect;

            private readonly GameObject gameObject;

            private readonly DiceMotionTracker motionTracker;

            private readonly DiceValueTracker valueTracker;


            public bool IsResting => motionTracker.MotionState.IsResting;

            public Opt<UnityEvent> OnEffectExecuted => effect.Map(it => it.OnExecuted);


            public Dice(GameObject gameObject)
            {
                this.gameObject = gameObject;
                valueTracker = gameObject.GetComponent<DiceValueTracker>();
                motionTracker = gameObject.GetComponent<DiceMotionTracker>();
                effect = gameObject.TryGetComponent<DiceEffect>();
            }


            public void PreventEffects() =>
                effect.Iter(it => it.ExecutedThisRound = true);

            public void AllowEffects() =>
                effect.Iter(it => it.ExecutedThisRound = false);

        }

    }

}