using ComradeVanti.CSharpTools;
using TMPro;
using UnityEngine;

namespace Dev.ComradeVanti.Wurfel.UI
{

    public class ResultsDisplay : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI teamText;

        private ArenaKeeper arenaKeeper;
        private CameraController cameraController;

        private Opt<Team> firstWinner = Opt.None<Team>();

        private void Awake()
        {
            arenaKeeper = FindObjectOfType<ArenaKeeper>();
            cameraController = FindObjectOfType<CameraController>();
        }

        public void OnTeamReachedPoints(Team team)
        {
            if (firstWinner.IsNone())
            {
                gameObject.SetActive(true);
                cameraController.LookAt(transform.position);
                arenaKeeper.OnGameOver();
                teamText.text = $"{team} won.";
                firstWinner = Opt.Some(team);
            }
            else
                teamText.text = "Its a draw!";
        }

    }

}