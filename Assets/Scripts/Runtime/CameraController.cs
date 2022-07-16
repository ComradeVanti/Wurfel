using System.Collections;
using ComradeVanti.CSharpTools;
using UnityEngine;

namespace Dev.ComradeVanti.Wurfel
{

    public class CameraController : MonoBehaviour
    {

        [SerializeField] private Transform cameraTransform;
        [SerializeField] private float distance;
        [SerializeField] private float height;
        [SerializeField] private float smoothTime;
        [SerializeField] private float maxMoveSpeed;
        [SerializeField] private float maxRotationSpeed;

        private Opt<Coroutine> followRoutine = Opt.None<Coroutine>();
        private Opt<Vector3> targetPosition = Opt.None<Vector3>();
        private Vector3 lookDirection;
        private Vector3 moveVelocity;
        private Vector3 rotateVelocity;


        private Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        private Vector3 Rotation
        {
            get => transform.forward;
            set => transform.forward = value;
        }


        private void Awake() =>
            StopFollowing();

        private void FixedUpdate() =>
            targetPosition.Iter(target =>
            {
                var offsetTarget = CalcOffsetTarget(target);

                Position = CalcNextPosition(offsetTarget);
                Rotation = CalcNextRotation(target);
            });

        private Vector3 CalcNextPosition(Vector3 target) =>
            Vector3.SmoothDamp(Position, target,
                               ref moveVelocity, smoothTime, maxMoveSpeed);

        private Vector3 CalcNextRotation(Vector3 target) =>
            Vector3.SmoothDamp(Rotation, (target - Position).normalized,
                               ref rotateVelocity, smoothTime, maxRotationSpeed);

        private Vector3 CalcOffsetTarget(Vector3 target)
        {
            var offset = (-lookDirection.Flat() * distance).WithY(height);
            return target + offset;
        }

        public void LookAt(Vector3 position, Vector3 lookDirection)
        {
            StopFollowing();
            this.lookDirection = lookDirection;
            targetPosition = Opt.Some(position);
        }

        public void Follow(Transform transform, Vector3 lookDirection)
        {
            IEnumerator Routine()
            {
                while (enabled)
                {
                    targetPosition = Opt.Some(transform.position);
                    yield return null;
                }
            }

            StopFollowing();
            this.lookDirection = lookDirection;
            followRoutine = Opt.Some(StartCoroutine(Routine()));
        }

        public void StopFollowing()
        {
            followRoutine.Iter(StopCoroutine);
            followRoutine = Opt.None<Coroutine>();
            targetPosition = Opt.None<Vector3>();
        }

    }

}