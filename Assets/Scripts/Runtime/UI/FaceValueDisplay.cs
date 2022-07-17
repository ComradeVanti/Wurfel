using TMPro;
using UnityEngine;

namespace Dev.ComradeVanti.Wurfel.UI
{

    public class FaceValueDisplay : MonoBehaviour
    {

        [SerializeField] private DiceFace face;
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
            DisplayedValue = face.Value;

    }

}