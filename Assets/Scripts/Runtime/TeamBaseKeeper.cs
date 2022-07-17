using UnityEngine;
using UnityEngine.Events;

namespace Dev.ComradeVanti.Wurfel
{

    public class TeamBaseKeeper : MonoBehaviour
    {

        [SerializeField] private UnityEvent onTurnStarts;
        [SerializeField] private Team team;
        [SerializeField] private DiceSpawner diceSpawner;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private UnityEvent<int> onScoreChanged;
        [SerializeField] private UnityEvent<Team> onScoreDone;

        private int score;
        private DiceLauncher launcher;


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
            this.score = Mathf.Max(this.score + score, 0);
            onScoreChanged.Invoke(this.score);
            
            if(score >= 50)
                onScoreDone.Invoke(team);
        }

        private void StartTurn()
        {
            cameraController.LookAt(transform.position + new Vector3(0, 1, 0));
            onTurnStarts.Invoke();
        }
        
        public void SpawnDice()
        {
            var diceGameObject = diceSpawner.SpawnDice(transform.position);
            launcher.Launch(diceGameObject);
            cameraController.Follow(diceGameObject.transform);
        }

    }

}