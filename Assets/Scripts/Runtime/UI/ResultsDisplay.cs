using TMPro;
using UnityEngine;

namespace Dev.ComradeVanti.Wurfel.UI
{

    public class ResultsDisplay : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI teamText;

        private ArenaKeeper arenaKeeper;
        private CameraController cameraController;

        private void Awake()
        {
            arenaKeeper = FindObjectOfType<ArenaKeeper>();
            cameraController = FindObjectOfType<CameraController>();
        }

        public void OnTeamWon(Team team)
        {
            gameObject.SetActive(true);
            cameraController.LookAt(transform.position);
            arenaKeeper.OnGameOver();
            teamText.text = $"{team} won.";
        }

    }

}