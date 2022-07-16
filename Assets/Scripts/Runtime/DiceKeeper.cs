using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Dev.ComradeVanti.Wurfel
{

    public class DiceKeeper : MonoBehaviour
    {

        [SerializeField] private UnityEvent onAllDieBecameStill;

        private readonly List<Dice> die = new List<Dice>();


        public void OnDiceSpawned(GameObject diceGameObject)
        {
            var dice = new Dice(diceGameObject);
            die.Add(dice);
            StartCoroutine(WaitForDieToStopMoving());
        }

        private IEnumerator WaitForDieToStopMoving()
        {
            while (!die.All(it => it.IsResting)) yield return null;
            yield return new WaitForSeconds(1f);
            onAllDieBecameStill.Invoke();
        }


        private class Dice
        {

            private readonly DiceMotionTracker diceMotionTracker;

            private readonly GameObject gameObject;


            public bool IsResting => diceMotionTracker.MotionState.IsResting;


            public Dice(GameObject gameObject)
            {
                this.gameObject = gameObject;
                diceMotionTracker = gameObject.GetComponent<DiceMotionTracker>();
            }

        }

    }

}