using UnityEngine;

namespace Dev.ComradeVanti.Wurfel
{

    public class MotionFreezer : MonoBehaviour
    {

        private new Rigidbody rigidbody;

        private bool isFrozen;
        private Vector3 velocity;
        private Vector3 angularVelocity;


        public bool IsFrozen
        {
            get => isFrozen;
            set
            {
                if (value)
                    Freeze();
                else
                    Unfreeze();
            }
        }


        private void Awake() => 
            rigidbody = GetComponent<Rigidbody>();

        public void Freeze()
        {
            if (isFrozen) return;
            isFrozen = true;

            velocity = rigidbody.velocity;
            angularVelocity = rigidbody.angularVelocity;
            rigidbody.isKinematic = true;
        }

        public void Unfreeze()
        {
            if (!isFrozen) return;
            isFrozen = false;

            rigidbody.isKinematic = false;
            rigidbody.velocity = velocity;
            rigidbody.angularVelocity = angularVelocity;
        }

    }

}