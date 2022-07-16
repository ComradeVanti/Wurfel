using System;
using UnityEngine;
using UnityEngine.Events;

namespace Dev.ComradeVanti.Wurfel
{

    public class GameKeeper : MonoBehaviour
    {

        [SerializeField] private Team startingTeam;
        [SerializeField] private UnityEvent<Team> onTurnTeamChanged;

        private State gameState = new State(Team.Red);


        private void Start() =>
            SetTeam(startingTeam);

        public void SwitchTeam() =>
            SetTeam(gameState.Team == Team.Red ? Team.Blue : Team.Red);

        private void SetTeam(Team team)
        {
            MapState(_ => new State(team));
            onTurnTeamChanged.Invoke(team);
        }

        private void MapState(Func<State, State> mapF) =>
            gameState = mapF(gameState);


        private record State(Team Team);

    }

}