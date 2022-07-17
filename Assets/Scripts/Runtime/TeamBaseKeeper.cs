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

        private int score;
        private DiceLauncher launcher;


        private void Awake() =>
            launcher = GetComponentInChildren<DiceLauncher>();

        public void OnTurnTeamChanged(Team turnTeam)
        {
            if (turnTeam == team)
                StartTurn();
        }

        public void AddScore(int score)
        {
            this.score = Mathf.Max(this.score + score, 0);
            onScoreChanged.Invoke(this.score);
        }

        private void StartTurn()
        {
            cameraController.LookAt(transform.position + new Vector3(0, 1, 0));
            onTurnStarts.Invoke();
        }
        
        public void SpawnDice()
        {
            var diceName = diceSpawner.DiceNames.Random();
            SpawnDiceWithName(diceName);
        }

        private void SpawnDiceWithName(string diceName)
        {
            var diceGameObject = diceSpawner.SpawnDice(diceName, transform.position);
            launcher.Launch(diceGameObject);
            cameraController.Follow(diceGameObject.transform);
        }

    }

}