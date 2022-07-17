using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Dev.ComradeVanti.Wurfel
{

    public class DiceMotionTracker : MonoBehaviour
    {

        [SerializeField] private UnityEvent<bool> onIsTouchingSurfaceChanged;
        [SerializeField] private UnityEvent<bool> onIsMovingChanged;
        [SerializeField] private float stillMovementSpeedThreshold;
        [SerializeField] private float stillRotationSpeedThreshold;

        private new Rigidbody rigidbody;
        private readonly HashSet<Collider> touchingColliders = new HashSet<Collider>();
        private bool wasTouchingSurface;
        private bool wasMoving;


        private float Speed => rigidbody.velocity.magnitude;

        private float RotationSpeed => rigidbody.angularVelocity.magnitude;

        private bool IsMovingLaterally => Speed > stillMovementSpeedThreshold;

        private bool IsRotating => RotationSpeed > stillRotationSpeedThreshold;

        private bool IsMoving => IsMovingLaterally || IsRotating;

        private bool IsTouchingSurface => touchingColliders.Count > 0;


        private void Awake() =>
            rigidbody = GetComponent<Rigidbody>();

        private void Update() =>
            UpdateIsMoving();

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.relativeVelocity.magnitude > 0.1f)
            {
                touchingColliders.Add(collision.collider);
                UpdateIsTouchingSurface();
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.relativeVelocity.magnitude > 0.1f)
            {
                touchingColliders.Remove(collision.collider);
                UpdateIsTouchingSurface();
            }
        }

        private void UpdateIsMoving()
        {
            var isMoving = IsMoving;

            if (isMoving != wasMoving)
            {
                wasMoving = isMoving;
                onIsMovingChanged.Invoke(isMoving);
            }
        }

        private void UpdateIsTouchingSurface()
        {
            var isTouchingSurface = IsTouchingSurface;
            if (isTouchingSurface != wasTouchingSurface)
            {
                wasTouchingSurface = isTouchingSurface;
                onIsTouchingSurfaceChanged.Invoke(isTouchingSurface);
            }
        }

    }

}