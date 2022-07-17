using UnityEngine;
using UnityEngine.Events;

namespace Dev.ComradeVanti.Wurfel
{

    public class CollisionTrigger : MonoBehaviour
    {

        [SerializeField] private UnityEvent<bool> onTriggeredChanged;

        private int collisionCount;


        private int CollisionCount
        {
            get => collisionCount;
            set
            {
                if (CollisionCount != value)
                {
                    collisionCount = value;
                    onTriggeredChanged.Invoke(collisionCount != 0);
                }
            }
        }


        public void OnTriggerEnter(Collider other) =>
            CollisionCount++;

        private void OnTriggerExit(Collider other) =>
            CollisionCount--;

    }

}