using System.Collections;
using UnityEngine;

namespace Dev.ComradeVanti.Wurfel
{

    public class Grow : DiceEffect
    {

        [SerializeField] private float minSize;
        [SerializeField] private float maxSize;
        [SerializeField] private float minMass;
        [SerializeField] private float maxMass;
        [SerializeField] private float growTime;
        

        private float Size
        {
            get => transform.localScale.x;
            set => transform.localScale = new Vector3(value, value, value);
        }

        private float Mass
        {
            get => rigidbody.mass;
            set => rigidbody.mass = value;
        }
        

        protected override void Execute(int strength)
        {
            var t = Mathf.InverseLerp(1, 6, strength);
            var size = Mathf.Lerp(minSize, maxSize, t);
            var mass = Mathf.Lerp(minMass, maxMass, t);

            GrowToSize(size, mass);
        }

        private void GrowToSize(float size, float mass)
        {
            IEnumerator Animation()
            {
                var t = 0f;
                var startSize = Size;
                var startMass = Mass;

                CompleteEffect();
                
                while (t < 1)
                {
                    t = Mathf.MoveTowards(t, 1, Time.deltaTime / growTime);
                    Size = Mathf.Lerp(startSize, size, t);
                    Mass = Mathf.Lerp(startMass, mass, t);
                    yield return null;
                }
            }

            StartCoroutine(Animation());
        }

    }

}