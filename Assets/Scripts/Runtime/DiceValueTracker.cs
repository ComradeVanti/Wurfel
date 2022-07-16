using UnityEngine;
using UnityEngine.Events;

namespace Dev.ComradeVanti.Wurfel
{

    public class DiceValueTracker : MonoBehaviour
    {

        [SerializeField] private UnityEvent<int> onValueChanged;

        private DiceValuePoint[] valuePoints;


        private DiceValuePoint TopValuePoint => valuePoints.FirstBy(it => it.Y);

        public int Value => TopValuePoint.Value;


        private void Awake() =>
            valuePoints = GetComponentsInChildren<DiceValuePoint>();

        public void OnIsStillChanged(bool isStill)
        {
            if (isStill)
                onValueChanged.Invoke(Value);
        }

    }

}