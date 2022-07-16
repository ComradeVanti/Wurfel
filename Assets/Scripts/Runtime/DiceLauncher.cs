using System.Collections;
using ComradeVanti.CSharpTools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Dev.ComradeVanti.Wurfel
{

    public class DiceLauncher : MonoBehaviour
    {

        [SerializeField] private UnityEvent<Opt<float>> onLaunchForceChanged;
        [SerializeField] private UnityEvent onDiceLaunched;
        [SerializeField] private SpringJoint spring;
        [SerializeField] private float tumbleForce;
        [SerializeField] private float launchAngleRange;
        [SerializeField] private float minLaunchForce;
        [SerializeField] private float maxLaunchForce;
        [SerializeField] private float launchForceChargeTime;

        private Opt<Rigidbody> tumbleBody;


        private static float ScreenWidth => Screen.width;

        private static (float min, float max) LeftRightPixelRange
        {
            get
            {
                var center = ScreenWidth / 2f;
                var range = ScreenWidth * 0.75f;
                var extent = range / 2f;
                return (center - extent, center + extent);
            }
        }

        private static float MouseX => Mouse.current.position.ReadValue().x;

        private static float MouseT
        {
            get
            {
                var (min, max) = LeftRightPixelRange;
                return Mathf.InverseLerp(min, max, MouseX);
            }
        }

        private float LaunchAngle => Mathf.Lerp(-launchAngleRange, launchAngleRange, MouseT);

        private Vector3 LaunchDir => Quaternion.AngleAxis(LaunchAngle, Vector3.up) * transform.forward;


        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame && tumbleBody.IsSome())
                tumbleBody.Iter(Launch);
        }

        private void FixedUpdate() =>
            tumbleBody.Iter(body =>
            {
                var direction = new Vector3(
                    Mathf.PingPong(Time.time, 1),
                    Mathf.PingPong(Time.time + 0.5f, 1),
                    Mathf.PingPong(Time.time + 0.75f, 1));
                body.AddTorque(direction * tumbleForce);
            });

        private void Launch(Rigidbody diceRigidbody)
        {
            void LaunchWith(float force)
            {
                spring.connectedBody = null;
                diceRigidbody.AddForce(LaunchDir * force, ForceMode.Impulse);
                tumbleBody = Opt.None<Rigidbody>();
                onDiceLaunched.Invoke();
            }

            IEnumerator ChargeForce()
            {
                var t = 0f;
                while (Mouse.current.leftButton.isPressed)
                {
                    t = Mathf.MoveTowards(t, 1f, Time.deltaTime / launchForceChargeTime);
                    onLaunchForceChanged.Invoke(Opt.Some(t));
                    yield return null;
                }

                onLaunchForceChanged.Invoke(Opt.None<float>());

                var force = Mathf.Lerp(minLaunchForce, maxLaunchForce, t);
                LaunchWith(force);
            }

            StartCoroutine(ChargeForce());
        }

        public void PrepareForLaunching(GameObject diceGameObject)
        {
            var diceRigidbody = diceGameObject.GetComponent<Rigidbody>();

            tumbleBody = Opt.Some(diceRigidbody);
            spring.connectedBody = diceRigidbody;
        }

    }

}