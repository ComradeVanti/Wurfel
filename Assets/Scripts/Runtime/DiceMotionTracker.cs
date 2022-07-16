using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Dev.ComradeVanti.Wurfel
{

    public class DiceMotionTracker : MonoBehaviour
    {

        [SerializeField] private LayerMask groundLayers;
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private float stillMovementSpeedThreshold;
        [SerializeField] private float stillRotationSpeedThreshold;
        [SerializeField] private UnityEvent<DiceMotionState> onMotionChanged;

        private DiceMotionState prevMotionState = new DiceMotionState(true, DiceGroundedState.InAir);
        private int touchingGroundSurfacesCount;
        private Transform[] faceTransforms;


        private float Speed => rigidbody.velocity.magnitude;

        private float RotationSpeed => rigidbody.angularVelocity.magnitude;

        private bool IsMovingLaterally => Speed > stillMovementSpeedThreshold;

        private bool IsRotating => RotationSpeed > stillRotationSpeedThreshold;

        private bool IsMoving => IsMovingLaterally || IsRotating;

        private bool IsOnGround => touchingGroundSurfacesCount > 0;

        private bool IsFlatOnGround => faceTransforms.Any(FaceIsGrounded);

        private DiceGroundedState GroundedState =>
            IsOnGround
                ? IsFlatOnGround
                    ? DiceGroundedState.FlatOnGround
                    : DiceGroundedState.TouchingGround
                : DiceGroundedState.InAir;

        public DiceMotionState MotionState =>
            new DiceMotionState(IsMoving, GroundedState);

        private void Awake() =>
            faceTransforms = GetComponentsInChildren<DiceValuePoint>()
                             .Select(it => it.transform)
                             .ToArray();

        private void Update()
        {
            var motionState = MotionState;

            if (motionState != prevMotionState)
            {
                onMotionChanged.Invoke(motionState);
                prevMotionState = motionState;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (groundLayers.Contains(collision.gameObject.layer))
                touchingGroundSurfacesCount++;
        }

        private void OnCollisionExit(Collision collision)
        {
            if (groundLayers.Contains(collision.gameObject.layer))
                touchingGroundSurfacesCount--;
        }

        private bool FaceIsGrounded(Transform faceTransform) =>
            Physics.CheckSphere(faceTransform.position, 0.01f, groundLayers);

    }

}