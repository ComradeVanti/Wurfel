using System.Collections;
using UnityEngine;

namespace Dev.ComradeVanti.Wurfel
{

    public class AddScore : DiceEffect
    {

        [SerializeField] private int multiplier;

        private ScoreAdder scoreAdder;


        protected override void Awake()
        {
            base.Awake();
            scoreAdder = FindObjectOfType<ScoreAdder>();
        }

        protected override void Execute(int strength)
        {
            IEnumerator Routine()
            {
                var score = strength * multiplier;
                yield return scoreAdder.AddScore(score, transform.position);
                CompleteEffect();
            }

            StartCoroutine(Routine());
        }

    }

}