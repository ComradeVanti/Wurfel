using UnityEngine;
using UnityEngine.Events;

namespace Dev.ComradeVanti.Wurfel
{

    public abstract class DiceEffect : MonoBehaviour
    {

        [SerializeField] protected UnityEvent onDone;

        public abstract void Activate(int strength);

    }

}