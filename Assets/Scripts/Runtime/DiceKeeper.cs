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
            StartCoroutine(WaitForDieToBecomeStill());
        }

        private IEnumerator WaitForDieToBecomeStill()
        {
            while (die.Any(it => !it.IsStill)) yield return null;
            yield return new WaitForSeconds(1f);
            onAllDieBecameStill.Invoke();
        }


        private class Dice
        {

            private readonly GameObject gameObject;
            private readonly StillnessDetector stillnessDetector;


            public bool IsStill => stillnessDetector.IsStill;


            public Dice(GameObject gameObject)
            {
                this.gameObject = gameObject;
                stillnessDetector = gameObject.GetComponent<StillnessDetector>();
            }

        }

    }

}