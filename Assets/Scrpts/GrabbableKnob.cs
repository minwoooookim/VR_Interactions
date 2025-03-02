using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Oculus.Interaction
{
    public class GrabbableKnob : MonoBehaviour, ITransformer
    {
        [SerializeField, Optional] private Transform _pivotTransform = null;
        [SerializeField] private Vector3 referencePoint;
        [SerializeField] private OneGrabRotateConstraints _constraints;
        [Serializable] public class OneGrabRotateConstraints
        {
            public FloatConstraint MinAngle;
            public FloatConstraint MaxAngle;
        }

        public OneGrabRotateConstraints Constraints
        {
            get { return _constraints; }
            set { _constraints = value; }
        }

        private float _relativeAngle = 0.0f;
        private float _constrainedRelativeAngle = 0.0f;
        private float _currentValue = 0.0f; // Slider value (0~1)

        private IGrabbable _grabbable;

        private Pose _initialPose;
        private Pose _previousGrabPose;

        [SerializeField] private Slider targetSlider;

        // [CHANGED] 회전 민감도(Twist Sensitivity)를 조절하기 위한 변수
        [SerializeField]
        private float _twistSensitivity = 1.0f;

        public void SetKnobValue(float value)
        {
            _currentValue = value;
            UpdateRotationFromValue();
        }

        private void UpdateRotationFromValue()
        {
            float minAngleConstraint = _constraints.MinAngle.Constrain ? _constraints.MinAngle.Value : 0f;
            float maxAngleConstraint = _constraints.MaxAngle.Constrain ? _constraints.MaxAngle.Value : 360f;

            float targetAngle = Mathf.Lerp(minAngleConstraint, maxAngleConstraint, _currentValue);
            float angleDelta = targetAngle - _constrainedRelativeAngle;

            Transform pivot = _pivotTransform != null ? _pivotTransform : _grabbable.Transform;
            Vector3 rotationAxis = pivot.TransformDirection(Vector3.up);

            _constrainedRelativeAngle = targetAngle;
            _relativeAngle = targetAngle;

            _grabbable.Transform.RotateAround(pivot.position, rotationAxis, angleDelta);
        }

        public void Initialize(IGrabbable grabbable)
        {
            _grabbable = grabbable;
        }

        public void BeginTransform()
        {
            var grabPoint = _grabbable.GrabPoints[0];
            Transform pivot = _pivotTransform != null ? _pivotTransform : _grabbable.Transform;
            Vector3 rotationAxis = pivot.TransformDirection(Vector3.up);

            _initialPose = new Pose(
                pivot.position,
                Quaternion.LookRotation(pivot.position - grabPoint.position, rotationAxis)
            );
            _previousGrabPose = grabPoint;
        }

        public void UpdateTransform()
        {
            var grabPoint = _grabbable.GrabPoints[0];
            var targetTransform = _grabbable.Transform;

            Transform pivot = _pivotTransform != null ? _pivotTransform : targetTransform;
            Vector3 rotationAxis = pivot.TransformDirection(Vector3.up);

            var targetPose = grabPoint.GetTransformedBy(_initialPose);
            var previousPose = _previousGrabPose.GetTransformedBy(_initialPose);

            // [CHANGED] 회전 민감도(TwistSensitivity)를 적용하기 위해 rawAngleDelta를 구분
            var rawAngleDelta = Vector3.SignedAngle(targetPose.up, previousPose.up, _initialPose.forward);
            var angleDelta = rawAngleDelta * _twistSensitivity; // 민감도 적용

            float previousAngle = _constrainedRelativeAngle;

            _relativeAngle += angleDelta;
            _constrainedRelativeAngle = _relativeAngle;

            // Min/Max 각도 제약이 있으면 적용
            if (_constraints.MinAngle.Constrain)
            {
                _constrainedRelativeAngle = Mathf.Max(_constrainedRelativeAngle, _constraints.MinAngle.Value);
            }
            if (_constraints.MaxAngle.Constrain)
            {
                _constrainedRelativeAngle = Mathf.Min(_constrainedRelativeAngle, _constraints.MaxAngle.Value);
            }

            // 제약 후 실제로 적용되는 angleDelta를 다시 계산
            angleDelta = _constrainedRelativeAngle - previousAngle;

            // 회전 적용
            targetTransform.RotateAround(pivot.position, rotationAxis, angleDelta);

            // [CHANGED] 0~1 슬라이더 값(_currentValue)을 업데이트
            float minAngleConstraint = _constraints.MinAngle.Constrain ? _constraints.MinAngle.Value : 0f;
            float maxAngleConstraint = _constraints.MaxAngle.Constrain ? _constraints.MaxAngle.Value : 360f;

            if (Mathf.Abs(maxAngleConstraint - minAngleConstraint) < Mathf.Epsilon)
            {
                _currentValue = 0.0f;
            }
            else
            {
                _currentValue = Mathf.InverseLerp(minAngleConstraint, maxAngleConstraint, _constrainedRelativeAngle);
            }

            // [ADDED] 슬라이더가 할당되어 있다면 업데이트된 _currentValue를 반영하여 슬라이더의 value 업데이트
            if (targetSlider != null)
            {
                targetSlider.value = _currentValue;
            }

            _previousGrabPose = grabPoint;
        }

        public void EndTransform()
        {
            // Clamps relative angle to constraints to remove windup
            if (_constraints.MinAngle.Constrain)
            {
                _relativeAngle = Mathf.Max(_constrainedRelativeAngle, _constraints.MinAngle.Value);
            }
            if (_constraints.MaxAngle.Constrain)
            {
                _relativeAngle = Mathf.Min(_constrainedRelativeAngle, _constraints.MaxAngle.Value);
            }
        }

        #region Inject

        public void InjectOptionalPivotTransform(Transform pivotTransform)
        {
            _pivotTransform = pivotTransform;
        }

        public void InjectOptionalConstraints(OneGrabRotateConstraints constraints)
        {
            _constraints = constraints;
        }

        #endregion
    }
}
