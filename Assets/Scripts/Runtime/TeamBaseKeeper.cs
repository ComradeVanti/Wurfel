using UnityEngine;

namespace Dev.ComradeVanti.Wurfel
{

    public class TeamBaseKeeper : MonoBehaviour
    {

        [SerializeField] private Team team;
        [SerializeField] private DiceSpawner diceSpawner;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private DiceLauncher launcher;
        

        public void OnTurnTeamChanged(Team turnTeam)
        {
            if (turnTeam == team) 
                StartTurn();
        }

        public void StartTurn() => 
            SpawnDice();

        private void SpawnDice()
        {
            var diceGameObject = diceSpawner.SpawnDice(transform.position);
            launcher.Launch(diceGameObject);
            cameraController.Follow(diceGameObject.transform, transform.forward);
        }

    }

}