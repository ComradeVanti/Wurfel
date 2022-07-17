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
        [SerializeField] private UnityEvent<GameObject> onDiceLaunched;
        [SerializeField] private SpringJoint spring;
        [SerializeField] private float tumbleForce;
        [SerializeField] private float launchAngleRange;
        [SerializeField] private float minLaunchForce;
        [SerializeField] private float maxLaunchForce;
        [SerializeField] private float launchForceChargeTime;


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


        private Coroutine Hook(Rigidbody diceRigidbody)
        {
            IEnumerator Tumble()
            {
                while (enabled)
                {
                    var direction = new Vector3(
                        Mathf.PingPong(Time.time, 1),
                        Mathf.PingPong(Time.time + 0.5f, 1),
                        Mathf.PingPong(Time.time + 0.75f, 1));
                    diceRigidbody.AddTorque(direction * (tumbleForce * Time.deltaTime));
                    yield return null;
                }
            }

            spring.connectedBody = diceRigidbody;
            return StartCoroutine(Tumble());
        }

        private void Unhook(Coroutine tumbleRoutine)
        {
            StopCoroutine(tumbleRoutine);
            spring.connectedBody = null;
        }

        public void Launch(GameObject diceGameObject)
        {
            var diceRigidbody = diceGameObject.GetComponent<Rigidbody>();
            var tumbleRoutine = Hook(diceRigidbody);

            void LaunchWith(Vector3 force)
            {
                Unhook(tumbleRoutine);
                diceRigidbody.AddForce(force, ForceMode.Impulse);
                onDiceLaunched.Invoke(diceGameObject);
            }

            IEnumerator ChargeForce(Vector3 launchDirection)
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
                LaunchWith(launchDirection * force);
            }

            IEnumerator PickDirection()
            {
                while (!Mouse.current.leftButton.isPressed && enabled)
                    yield return null;
                StartCoroutine(ChargeForce(LaunchDir));
            }
            
            StartCoroutine(PickDirection());
        }

    }

}