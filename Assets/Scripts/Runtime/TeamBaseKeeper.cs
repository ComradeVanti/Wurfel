using UnityEngine;
using UnityEngine.Events;

namespace Dev.ComradeVanti.Wurfel
{

    public class TeamBaseKeeper : MonoBehaviour
    {

        [SerializeField] private UnityEvent onTurnStarts;
        [SerializeField] private Team team;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private UnityEvent<int> onScoreChanged;
        [SerializeField] private UnityEvent<Team> onScoreDone;

        private DiceLauncher launcher;
        private ArenaKeeper arenaKeeper;

        private void Awake()
        {
            launcher = GetComponentInChildren<DiceLauncher>();
            arenaKeeper = FindObjectOfType<ArenaKeeper>();
            onScoreChanged.Invoke(0);
        }


        public void CountScore()
        {
            var score = Mathf.Max(arenaKeeper.CountScoreFor(team), 0);
            onScoreChanged.Invoke(score);

            if (score > 20)
                onScoreDone.Invoke(team);
        }

        public void OnTurnTeamChanged(Team turnTeam)
        {
            if (turnTeam == team)
                StartTurn();
        }

        private void StartTurn()
        {
            cameraController.LookAt(transform.position + new Vector3(0, 1, 0));
            onTurnStarts.Invoke();
        }

        public void OnDiceSelected(GameObject dice)
        {
            dice.transform.position = transform.position;
            launcher.Launch(dice);
            cameraController.Follow(dice.transform);
        }

    }

}