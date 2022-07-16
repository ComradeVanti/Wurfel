using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Dev.ComradeVanti.Wurfel
{

    public class StillnessDetector : MonoBehaviour
    {
        
        [SerializeField] private LayerMask groundLayers;
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private float stillSpeedThreshold;
        [SerializeField] private UnityEvent<bool> onIsStillChanged;

        private bool prevIsStill;
        private Transform[] faceTransforms;
        
        
        private float Speed => rigidbody.velocity.magnitude;

        private bool IsMoving => Speed > stillSpeedThreshold;
        
        private bool IsGrounded => faceTransforms.Any(FaceIsGrounded);

        public bool IsStill => !IsMoving && IsGrounded;


        private void Awake() =>
            faceTransforms = GetComponentsInChildren<DiceValuePoint>()
                             .Select(it => it.transform)
                             .ToArray();

        private void Update()
        {
            var isStill = IsStill;

            if (isStill != prevIsStill)
            {
                onIsStillChanged.Invoke(isStill);
                prevIsStill = isStill;
            }
        }

        private bool FaceIsGrounded(Transform faceTransform) =>
            Physics.CheckSphere(faceTransform.position, 0.01f, groundLayers);

    }

}