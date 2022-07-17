using UnityEngine;

namespace Dev.ComradeVanti.Wurfel
{

    public class DiceSpawner : MonoBehaviour
    {

        [SerializeField] private GameObject prefab;


        public GameObject SpawnDice(Vector3 location) => 
            Instantiate(prefab, location, Quaternion.identity);

    }

}