using UnityEngine;

namespace Dev.ComradeVanti.Wurfel
{

    public class ContactBounce : MonoBehaviour
    {

        [SerializeField] public float bounceStrength;


        private Vector3 BounceForce => transform.up * bounceStrength;


        private void OnCollisionEnter(Collision collision) =>
            collision.rigidbody.AddForce(BounceForce, ForceMode.Impulse);

    }

}