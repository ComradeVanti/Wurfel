using System;
using UnityEngine;

namespace Dev.ComradeVanti.Wurfel
{

    public class DiceFace : MonoBehaviour
    {

        [SerializeField] private int value;
        [SerializeField] private DiceFace opposite;

        private int triggeredCount;

        
        public int Value => value;

        public bool IsFlatOnGround => triggeredCount == 4;
        
        public bool IsTouchingGround => triggeredCount > 0;

        public int OppositeValue => opposite.value;
        

        public void OnPointTriggeredChanged(bool isTriggered)
        {
            if (isTriggered) triggeredCount++;
            else triggeredCount--;
        }

    }

}