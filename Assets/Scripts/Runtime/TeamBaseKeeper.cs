using UnityEngine;

namespace Dev.ComradeVanti.Wurfel
{

    public class TeamBaseKeeper : MonoBehaviour
    {

        [SerializeField] private Team team;
        [SerializeField] private DiceSpawner diceSpawner;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private DiceLauncher launcher;

        private readonly DiceBag diceBag = DiceBag.MakeDefault();
        

        public void OnTurnTeamChanged(Team turnTeam)
        {
            if (turnTeam == team)
                StartTurn();
        }

        private void StartTurn() =>
            SpawnDice();

        private void SpawnDice()
        {
            var diceName = diceBag.GetRandom();
            var diceGameObject = diceSpawner.SpawnDice(diceName, transform.position);
            launcher.Launch(diceGameObject);
            cameraController.Follow(diceGameObject.transform);
        }

    }

}