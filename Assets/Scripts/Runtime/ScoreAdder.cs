using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

namespace Dev.ComradeVanti.Wurfel
{

    public class ScoreAdder : MonoBehaviour
    {

        [SerializeField] private Transform cameraTransform;
        [SerializeField] private UnityEvent<int, Team> onScoreAdded;
        [SerializeField] private TeamBaseKeeper redTeam;
        [SerializeField] private TeamBaseKeeper blueTeam;
        [SerializeField] private float riseHeight;
        [SerializeField] private float riseTime;
        
        
        public Coroutine AddScore(int score, Vector3 dicePosition)
        {
            IEnumerator Animation()
            {
                var team = dicePosition.x > 0 ? Team.Blue : Team.Red;
                var keeper = team == Team.Red ? redTeam : blueTeam;
                var total = Mathf.Max(keeper.Score + score, 0);
                
                onScoreAdded.Invoke(total, team);

                var t = 0f;
                var transform = this.transform;
                
                while (t < 1)
                {
                    t = Mathf.MoveTowards(t, 1, Time.deltaTime / riseTime);
                    var dY = Mathf.Lerp(0, riseHeight, t);
                    
                    transform.position = dicePosition.MapY(y => y + dY);
                    transform.forward = (transform.position- cameraTransform.position).normalized;
                    
                    yield return null;
                }

                keeper.AddScore(score);
                transform.position = new Vector3(0, -100, 0);
            }
            
            return StartCoroutine(Animation());
        }

    }

}