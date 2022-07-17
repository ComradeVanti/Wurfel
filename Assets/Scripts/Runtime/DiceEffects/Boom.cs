using System.Collections.Generic;
using System.Linq;
using ComradeVanti.OptUnity;
using UnityEngine;

namespace Dev.ComradeVanti.Wurfel
{

    public class Boom : DiceEffect
    {

        [SerializeField] private LayerMask affectedLayers;
        [SerializeField] private new ParticleSystem particleSystem;
        [SerializeField] private float maxRadius;
        [SerializeField] private float maxForce;


        public override void Activate(int strength)
        {
            var t = Mathf.InverseLerp(0, 6, strength);
            var radius = Mathf.Lerp(0, maxRadius, t);
            var force = Mathf.Lerp(0, maxForce, t);

            Explode(radius, force);
        }

        private void Explode(float radius, float force)
        {
            var affectedObjects = FindTargetsInRange(radius);

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

        private IEnumerable<Rigidbody> FindTargetsInRange(float radius) =>
            Physics.OverlapSphere(transform.position, radius, affectedLayers)
                   .Select(coll => coll.TryGetComponent<Rigidbody>())
                   .Collect()
                   .Where(it => it.gameObject != gameObject);

    }

}