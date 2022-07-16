using UnityEngine;

namespace Dev.ComradeVanti.Wurfel
{

    public class TeamBaseKeeper : MonoBehaviour
    {

        [SerializeField] private Team team;
        [SerializeField] private DiceSpawner diceSpawner;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private DiceLauncher launcher;
        [SerializeField] private Vector3 spawnOffset;


        private Vector3 SpawnPosition => transform.position + spawnOffset;


        public void OnTurnTeamChanged(Team turnTeam)
        {
            if (turnTeam == team) 
                StartTurn();
        }

        public void StartTurn() => 
            SpawnDice();

        private void SpawnDice()
        {
            var diceGameObject = diceSpawner.SpawnDice(SpawnPosition);
            launcher.PrepareForLaunching(diceGameObject);
            cameraController.Follow(diceGameObject.transform, transform.forward);
        }

    }

}