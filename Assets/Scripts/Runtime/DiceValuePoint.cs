using UnityEngine;

namespace Dev.ComradeVanti.Wurfel
{

    public class DiceValuePoint : MonoBehaviour
    {

        [SerializeField] private int value;


        public float Y => transform.position.y;
        

        public int Value => value;

    }

}