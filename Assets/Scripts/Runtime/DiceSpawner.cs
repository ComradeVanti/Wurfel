using UnityEngine;
using UnityEngine.Events;

namespace Dev.ComradeVanti.Wurfel
{

    public class DiceSpawner : MonoBehaviour
    {

        [SerializeField] private UnityEvent<GameObject> onDiceSpawned;
        [SerializeField] private GameObject prefab;


        public GameObject SpawnDice(Vector3 location)
        {
            var dice = Instantiate(prefab, location, Quaternion.identity);
            onDiceSpawned.Invoke(dice);
            return dice;
        }

    }

}