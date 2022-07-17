using TMPro;
using UnityEngine;

namespace Dev.ComradeVanti.Wurfel.UI
{
    public class ScoreAddedDisplay : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI display;
        [SerializeField] private Color redColor;
        [SerializeField] private Color blueColor;
        
        public void OnScoreAdded(int score, Team team)
        {
            display.text = score.ToString();
            display.color = team == Team.Red ? redColor : blueColor;
        }
    }
}
