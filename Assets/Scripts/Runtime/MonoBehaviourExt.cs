using System;
using System.Collections;
using UnityEngine;

namespace Dev.ComradeVanti.Wurfel
{

    public static class MonoBehaviourExt
    {

        public static Coroutine WaitAndRun(this MonoBehaviour monoBehaviour, float delay, Action action)
        {
            IEnumerator Routine()
            {
                yield return new WaitForSeconds(delay);
                action();
            }

            return monoBehaviour.StartCoroutine(Routine());
        }

    }

}