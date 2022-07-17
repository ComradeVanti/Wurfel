using ComradeVanti.CSharpTools;
using UnityEngine;
using UnityEngine.Events;

namespace Dev.ComradeVanti.Wurfel
{

    public class DiceFaceTracker : MonoBehaviour
    {

        [SerializeField] private UnityEvent<int> onValueChanged;

        private int lastValue;
        private DiceFace[] faces;


        private Opt<DiceFace> FlatGroundFace =>
            faces.TryFirst(face => face.IsFlatOnGround);

        private Opt<int> Value => FlatGroundFace.Map(it => it.OppositeValue);

        private void Awake() =>
            faces = GetComponentsInChildren<DiceFace>();

        private void Update() =>
            CheckIfValueChanged();

        private void CheckIfValueChanged() =>
            Value.Iter(value =>
            {
                if (value != lastValue)
                {
                    lastValue = value;
                    onValueChanged.Invoke(value);
                }
            });

    }

}