using System.Collections;
using UnityEngine;

namespace Dev.ComradeVanti.Wurfel
{

    public abstract class DiceEffect : MonoBehaviour
    {

        protected Dice dice;
        protected CameraController cameraController;
        protected ArenaKeeper arenaKeeper;
        

        protected virtual void Awake()
        {
            dice = GetComponent<Dice>();
            cameraController = FindObjectOfType<CameraController>();
            arenaKeeper = FindObjectOfType<ArenaKeeper>();
        }


        public virtual void Activate(int strength)
        {
            IEnumerator Routine()
            {
                arenaKeeper.IsFrozen = true;
                yield return CallCameraToMe();
                yield return new WaitForSeconds(0.5f);
                Execute(strength);
            }

            StartCoroutine(Routine());
        }

        protected virtual void Execute(int strength) { }

        protected Coroutine CallCameraToMe() =>
            cameraController.LookAt(transform.position);

        protected virtual void CompleteEffect()
        {
            arenaKeeper.IsFrozen = false;
            dice.OnEffectDone();
        }

    }

}