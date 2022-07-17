using UnityEngine;

namespace Dev.ComradeVanti.Wurfel
{

    public class Sticky : MonoBehaviour
    {

        [SerializeField] private LayerMask affectedLayers;

        private void OnCollisionEnter(Collision collision)
        {
            if (!affectedLayers.Contains(collision.gameObject.layer))
                return;

            var joint = gameObject.AddComponent<FixedJoint>();
            joint.enableCollision = false;
            joint.connectedBody = collision.rigidbody;
        }

    }

}