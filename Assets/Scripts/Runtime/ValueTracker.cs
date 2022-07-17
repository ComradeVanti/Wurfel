using ComradeVanti.CSharpTools;
using UnityEngine;
using UnityEngine.Events;

namespace Dev.ComradeVanti.Wurfel
{

    public class ValueTracker : MonoBehaviour
    {

        [SerializeField] private UnityEvent<Opt<int>> onValueChanged;

        private Opt<int> lastValue = Opt.None<int>();


        private void Update() =>
            UpdateValue();

        private void UpdateValue()
        {
            var value = GetCurrentValue();

            if (!value.Equals(lastValue))
            {
                lastValue = value;
                onValueChanged.Invoke(value);
            }
        }

        private Opt<int> GetCurrentValue()
        {
            if (IsPointingUp(transform.forward))
                return Opt.Some(1);
            if (IsPointingUp(transform.up))
                return Opt.Some(2);
            if (IsPointingUp(-transform.right))
                return Opt.Some(3);
            if (IsPointingUp(transform.right))
                return Opt.Some(4);
            if (IsPointingUp(-transform.up))
                return Opt.Some(5);
            if (IsPointingUp(-transform.forward))
                return Opt.Some(6);
            return Opt.None<int>();
        }

        private static bool IsPointingUp(Vector3 direction)
        {
            var distance = Vector3.Distance(direction, Vector3.up);
            return distance < 0.1f;
        }

    }

}