using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Dev.ComradeVanti.Wurfel
{

    public class ArenaKeeper : MonoBehaviour
    {

        [SerializeField] private UnityEvent onRoundEnded;

        private readonly List<Dice> die = new List<Dice>();
        private bool gameRunning = true;


        public bool IsFrozen
        {
            set => die.Iter(it => it.IsFrozen = value);
        }


        public void OnDiceLaunched(GameObject diceGameObject) =>
            AddDice(diceGameObject.GetComponent<Dice>());

        public int CountScoreFor(Team team)
        {
            bool Counts(Dice dice)
            {
                var x = dice.transform.position.x;
                return team == Team.Red ? x < 0 : x > 0;
            }

            var dieToCount = die.Where(Counts);

            return dieToCount.Select(it => it.ScoreValue).Collect().Sum();
        }

        private bool ArenaIsCalm() =>
            die.All(it => it.IsResting);

        public void OnGameOver()
        {
            IsFrozen = true;
            die.Clear();
            gameRunning = false;
        }

        public void RemoveDice(Dice dice)
        {
            die.Remove(dice);
            Destroy(dice.gameObject);
        }

        private void AddDice(Dice dice)
        {
            IEnumerator WaitForDiceToTouchGround()
            {
                while (dice && !dice.IsTouchingSurface)
                    yield return null;

                if (dice)
                    StartRound();
                else
                    onRoundEnded.Invoke();
            }

            die.Add(dice);
            StartCoroutine(WaitForDiceToTouchGround());
        }

        private void StartRound()
        {
            IEnumerator WaitForArenaToCalmDown()
            {
                while (true)
                {
                    if (ArenaIsCalm())
                    {
                        yield return new WaitForSeconds(1);
                        if (ArenaIsCalm())
                            break;
                    }

                    yield return null;
                }

                yield return new WaitForSeconds(1);
                EndRound();
            }

            die.Iter(it => it.OnNewRoundStarted());
            StartCoroutine(WaitForArenaToCalmDown());
        }

        private void EndRound()
        {
            die.Iter(it => it.OnRoundEnded());
            if (gameRunning)
                onRoundEnded.Invoke();
        }

    }

}