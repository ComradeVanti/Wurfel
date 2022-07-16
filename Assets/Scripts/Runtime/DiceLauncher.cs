using UnityEngine;
using UnityEngine.InputSystem;

namespace Dev.ComradeVanti.Wurfel
{

    public class DiceLauncher : MonoBehaviour
    {

        [SerializeField] private CameraController cameraController;
        [SerializeField] private SpringJoint spring;
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private float tumbleForce;
        [SerializeField] private float launchAngleRange;
        [SerializeField] private float launchForce;


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

        private Vector3 LaunchDir => Quaternion.AngleAxis(LaunchAngle, Vector3.up) * Vector2.right;


        private void Awake() => 
            cameraController.Follow(rigidbody.transform);

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
                Launch();
        }

        private void FixedUpdate()
        {
            if (rigidbody)
            {
                var direction = new Vector3(
                    Mathf.PingPong(Time.time, 1),
                    Mathf.PingPong(Time.time + 0.5f, 1),
                    Mathf.PingPong(Time.time + 0.75f, 1));
                rigidbody.AddTorque(direction * tumbleForce);
            }
        }

        private void Launch()
        {
            spring.connectedBody = null;
            var force = LaunchDir * launchForce;
            rigidbody.AddForce(force, ForceMode.Impulse);
            rigidbody = null;
        }

    }

}