using System.Linq;
using ComradeVanti.CSharpTools;
using UnityEngine;
using UnityEngine.Events;

namespace Dev.ComradeVanti.Wurfel
{

    public class DiceMotionTracker : MonoBehaviour
    {
        
        [SerializeField] private float stillMovementSpeedThreshold;
        [SerializeField] private float stillRotationSpeedThreshold;
        [SerializeField] private UnityEvent<DiceMotionChange> onMotionChanged;

        private new Rigidbody rigidbody;
        private MotionFreezer freezer;
        private Opt<DiceMotionState> prevMotionState = Opt.None<DiceMotionState>();
        private DiceFace[] faces;


        private float Speed => rigidbody.velocity.magnitude;

        private float RotationSpeed => rigidbody.angularVelocity.magnitude;

        private bool IsMovingLaterally => Speed > stillMovementSpeedThreshold;

        private bool IsRotating => RotationSpeed > stillRotationSpeedThreshold;

        private bool IsMoving => IsMovingLaterally || IsRotating;

        private bool IsOnGround => faces.Any(face => face.IsTouchingGround);

        private bool IsFlatOnGround => faces.Any(face => face.IsFlatOnGround);

        private DiceGroundedState GroundedState =>
            IsOnGround
                ? IsFlatOnGround
                    ? DiceGroundedState.FlatOnGround
                    : DiceGroundedState.TouchingGround
                : DiceGroundedState.InAir;


        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            freezer = GetComponent<MotionFreezer>();
            faces = GetComponentsInChildren<DiceFace>();
        }

        private void Update()
        {
            if (!freezer.IsFrozen)
                CheckForMotionUpdates();
        }

        private void CheckForMotionUpdates()
        {
            var motionState = new DiceMotionState(IsMoving, GroundedState);

            if (!prevMotionState.Contains(motionState))
            {
                var change = new DiceMotionChange(motionState, prevMotionState);
                onMotionChanged.Invoke(change);
                prevMotionState = Opt.Some(motionState);
            }
        }

    }

}