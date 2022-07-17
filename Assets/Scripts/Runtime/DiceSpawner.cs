using System;
using UnityEngine;

namespace Dev.ComradeVanti.Wurfel
{

    public class DiceSpawner : MonoBehaviour
    {

        [SerializeField] private Weighted<GameObject>[] prefabs;


        private GameObject GetRandomPrefab() =>
            prefabs.RandomElementByWeight(it => it.Weight).Item;

        public GameObject SpawnDice(Vector3 location)
        {
            var prefab = GetRandomPrefab();
            return Instantiate(prefab, location, Quaternion.identity);
        }


        [Serializable]
        public class Weighted<T>
        {

            [SerializeField] private T item;
            [SerializeField] private float weight;

            public T Item => item;

            public float Weight => weight;

        }

    }

}