using TMPro;
using UnityEngine;

namespace Dev.ComradeVanti.Wurfel.UI
{

    public class ScoreDisplay : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI scoreDisplay;


        public void OnScoreChanged(int score) => 
            scoreDisplay.text = score.ToString();

    }

}