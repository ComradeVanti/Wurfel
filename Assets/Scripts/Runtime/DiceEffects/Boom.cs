using System.Linq;
using UnityEngine;

namespace Dev.ComradeVanti.Wurfel
{

    public class Boom : DiceEffect
    {

        [SerializeField] private LayerMask affectedLayers;
        [SerializeField] private new ParticleSystem particleSystem;
        [SerializeField] private float maxForce;


        public override void Activate(int strength)
        {
            var t = Mathf.InverseLerp(0, 6, strength);
            var force = Mathf.Lerp(0, maxForce, t);

            Explode(force);
        }

        private void Explode(float force)
        {
            var affectedObjects = FindObjectsOfType<Rigidbody>()
                .Where(it => affectedLayers.Contains(it.gameObject.layer))
                .Where(it => it.gameObject != gameObject);

            onDone.Invoke();

            affectedObjects.Iter(rigidbody =>
            {
                var diff = rigidbody.position - transform.position;
                var distance = diff.magnitude;
                var adjustedForce = force / (distance / 2f);
                var direction = diff.normalized;

                rigidbody.AddForce(direction * adjustedForce, ForceMode.Impulse);
            });

            particleSystem.Play();
        }

    }

}