using System.Linq;
using UnityEngine;

namespace Dev.ComradeVanti.Wurfel
{

    public class DiceSpawner : MonoBehaviour
    {

        [SerializeField] private GameObject[] prefabs;


        public string[] DiceNames { get; private set; }


        private void Awake() =>
            DiceNames = prefabs.Select(prefab => prefab.name)
                               .Select(name => name[..^4])
                               .ToArray();

        private GameObject GetPrefabWithName(string diceName) =>
            prefabs.First(it => it.name == $"{diceName}Dice");

        public GameObject SpawnDice(string diceName, Vector3 location)
        {
            var prefab = GetPrefabWithName(diceName);
            return Instantiate(prefab, location, Quaternion.identity);
        }

    }

}