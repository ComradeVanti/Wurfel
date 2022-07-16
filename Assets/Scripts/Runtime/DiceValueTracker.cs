using System;
using UnityEngine;

namespace Dev.ComradeVanti.Wurfel
{

    public class DiceValueTracker : MonoBehaviour
    {

        private DiceValuePoint[] valuePoints;


        private DiceValuePoint TopValuePoint => valuePoints.FirstBy(it => it.Y);

        public int Value => TopValuePoint.Value;


        private void Awake() =>
            valuePoints = GetComponentsInChildren<DiceValuePoint>();
    }

}