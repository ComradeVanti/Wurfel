using TMPro;
using UnityEngine;

namespace Dev.ComradeVanti.Wurfel.UI
{

    public class FaceValueDisplay : MonoBehaviour
    {

        [SerializeField] private DiceValuePoint valuePoint;
        [SerializeField] private TextMeshProUGUI valueDisplay;


        private string ValueText
        {
            set => valueDisplay.text = value;
        }

        private int DisplayedValue
        {
            set => ValueText = value.ToString();
        }


        private void Awake() =>
            RefreshDisplay();

        private void RefreshDisplay() =>
            DisplayedValue = valuePoint.Value;

    }

}