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

        [SerializeField] private UnityEvent onAllDieBecameStill;

        private readonly List<Dice> die = new List<Dice>();


        public void OnDiceLaunched() =>
            AllowEffects();

        private void AllowEffects() => die.Iter(it => it.AllowEffects());

        private void PreventEffects() => die.Iter(it => it.PreventEffects());

        public void OnDiceSpawned(GameObject diceGameObject)
        {
            var dice = new Dice(diceGameObject);
            die.Add(dice);
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

            private readonly DiceMotionTracker diceMotionTracker;

            private readonly Opt<DiceEffect> effect;

            private readonly GameObject gameObject;


            public bool IsResting => diceMotionTracker.MotionState.IsResting;


            public Dice(GameObject gameObject)
            {
                this.gameObject = gameObject;
                diceMotionTracker = gameObject.GetComponent<DiceMotionTracker>();
                effect = gameObject.TryGetComponent<DiceEffect>();
            }


            public void PreventEffects() =>
                effect.Iter(it => it.ExecutedThisRound = true);

            public void AllowEffects() =>
                effect.Iter(it => it.ExecutedThisRound = false);

        }

    }

}