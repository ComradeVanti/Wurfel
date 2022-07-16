using ComradeVanti.CSharpTools;
using ComradeVanti.OptUnity;
using UnityEngine;

namespace Dev.ComradeVanti.Wurfel
{

    public class ContactBounce : MonoBehaviour
    {

        [SerializeField] public float bounceStrength;


        private Vector3 BounceForce => transform.up * bounceStrength;


        private void OnCollisionEnter(Collision collision) =>
            collision.gameObject
                     .TryGetComponent<Rigidbody>()
                     .Iter(rigidbody => rigidbody.AddForce(BounceForce, ForceMode.Impulse));

    }

}