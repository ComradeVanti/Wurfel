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
        
        
        public int Score { get; private set; }

        
        private void Awake()
        {
            launcher = GetComponentInChildren<DiceLauncher>();
            onScoreChanged.Invoke(0);
        }

        public void OnTurnTeamChanged(Team turnTeam)
        {
            if (turnTeam == team)
                StartTurn();
        }

        public void AddScore(int score)
        {
            Score = Mathf.Max(Score + score, 0);
            onScoreChanged.Invoke(Score);
            
            if(Score >= 50)
                onScoreDone.Invoke(team);
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