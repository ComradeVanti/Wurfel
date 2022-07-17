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


        public bool IsFrozen
        {
            set => die.Iter(it => it.IsFrozen = value);
        }


        public void OnDiceLaunched(GameObject diceGameObject) =>
            AddDice(diceGameObject.GetComponent<Dice>());

        private static bool IsCalm(Dice dice) =>
            !dice.IsMoving && !dice.IsActivatingEffect && !dice.IsFrozen;

        private bool ArenaIsCalm() =>
            die.All(IsCalm);

        public void RemoveDice(Dice dice)
        {
            die.Remove(dice);
            Destroy(dice.gameObject);
        }

        private void AddDice(Dice dice)
        {
            IEnumerator WaitForDiceToTouchGround()
            {
                while (dice && !dice.IsTouchingGround)
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
            onRoundEnded.Invoke();
        }

    }

}