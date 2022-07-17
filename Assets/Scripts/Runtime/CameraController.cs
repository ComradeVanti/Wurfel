using System;
using System.Collections;
using ComradeVanti.CSharpTools;
using UnityEngine;

namespace Dev.ComradeVanti.Wurfel
{

    public class CameraController : MonoBehaviour
    {

        [SerializeField] private float minDistance;
        [SerializeField] private float maxDistance;
        [SerializeField] private float height;
        [SerializeField] private float smoothTime;
        [SerializeField] private float maxMoveSpeed;
        [SerializeField] private float maxRotationSpeed;

        private Opt<Coroutine> followRoutine = Opt.None<Coroutine>();
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

        private float Distance
        {
            get
            {
                var t = Mathf.InverseLerp(0, maxMoveSpeed, moveVelocity.magnitude);
                return Mathf.Lerp(minDistance, maxDistance, t);
            }
        }


        private Vector3 CalcNextPosition(Vector3 target) =>
            Vector3.SmoothDamp(Position, target,
                               ref moveVelocity, smoothTime, maxMoveSpeed);

        private Vector3 CalcNextRotation(Vector3 target) =>
            Vector3.SmoothDamp(Rotation, target,
                               ref rotateVelocity, smoothTime, maxRotationSpeed);

        private Vector3 CalcOffsetTarget(Vector3 target)
        {
            var lookDirection = target.normalized;
            var offset = (lookDirection.Flat() * Distance).WithY(height);
            return target + offset;
        }

        public Coroutine FollowWith(Func<Vector3> calcTarget, Func<Vector3, Vector3, bool> shouldStop)
        {
            IEnumerator Move()
            {
                while (enabled)
                {
                    var target = calcTarget();

                    var offsetTarget = CalcOffsetTarget(target);
                    var targetRotation = (target - Position).normalized;

                    Position = CalcNextPosition(offsetTarget);
                    Rotation = CalcNextRotation(targetRotation);

                    if (shouldStop(offsetTarget, targetRotation))
                        break;
                    yield return null;
                }
            }

            StopFollowing();
            var routine = StartCoroutine(Move());
            followRoutine = Opt.Some(routine);
            return routine;
        }

        public Coroutine LookAt(Vector3 target)
        {
            bool ShouldStop(Vector3 offsetTarget, Vector3 targetRotation)
            {
                var reachedTarget = Vector3.Distance(Position, offsetTarget) < 0.1f;
                var looksAtTarget = Vector3.Distance(Rotation, targetRotation) < 0.1f;

                return reachedTarget && looksAtTarget;
            }

            return FollowWith(() => target, ShouldStop);
        }

        public Coroutine Follow(Transform transform) =>
            FollowWith(() => transform ? transform.position : Position,
                       (_, _) => !transform);

        public void StopFollowing()
        {
            followRoutine.Iter(it =>
            {
                if (it != null)
                    StopCoroutine(it);
            });
            followRoutine = Opt.None<Coroutine>();
        }

    }

}