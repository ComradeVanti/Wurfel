using System.Collections;
using ComradeVanti.CSharpTools;
using UnityEngine;

namespace Dev.ComradeVanti.Wurfel
{

    public class CameraController : MonoBehaviour
    {

        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Vector3 offset;
        [SerializeField] private float smoothTime;
        [SerializeField] private float maxSpeed;

        private Opt<Coroutine> followRoutine = Opt.None<Coroutine>();
        private Vector3 targetPosition;
        private Vector3 velocity;


        private Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        private Vector3 TargetPosition => targetPosition + offset;
        

        private void Awake() =>
            StopFollowing();

        private void FixedUpdate() =>
            Position = CalcNextPosition();

        private Vector3 CalcNextPosition() =>
            Vector3.SmoothDamp(Position, TargetPosition,
                               ref velocity, smoothTime, maxSpeed);

        public void LookAt(Vector3 position)
        {
            StopFollowing();
            targetPosition = position;
        }

        public void Follow(Transform transform)
        {
            IEnumerator Routine()
            {
                while (enabled)
                {
                    targetPosition = transform.position;
                    yield return null;
                }
            }

            StopFollowing();
            followRoutine = Opt.Some(StartCoroutine(Routine()));
        }

        public void StopFollowing()
        {
            followRoutine.Iter(StopCoroutine);
            followRoutine = Opt.None<Coroutine>();
            targetPosition = Position;
        }

    }

}