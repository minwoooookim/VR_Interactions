/************************************************************************************
Copyright : Copyright (c) Facebook Technologies, LLC and its affiliates. 
All rights reserved.

Your use of this SDK or tool is subject to the Oculus SDK License Agreement, available at
https://developer.oculus.com/licenses/oculussdk/

Unless required by applicable law or agreed to in writing, the Utilities SDK distributed
under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
ANY KIND, either express or implied. See the License for the specific language governing
permissions and limitations under the License.
************************************************************************************/

using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Oculus.Interaction
{
    /// <summary>
    /// A Transformer that rotates the target about the up axis.
    /// Updates apply relative rotational changes of a GrabPoint about the up vector
    /// of the pivot.
    /// 
    /// [CHANGED]
    /// Inspector에서 설정한 슬라이더(_currentValue) 값이 초기값으로 반영되도록 Awake()에서 기본 상태를 저장한 후,
    /// UpdateRotationFromValue()를 호출하여 오브젝트의 회전과 위치를 갱신합니다.
    /// </summary>
    public class GrabbableKnob : MonoBehaviour, ITransformer
    {
        [SerializeField, Optional]
        private Transform _pivotTransform = null;

        [SerializeField]
        private Vector3 referencePoint;

        [Serializable]
        public class OneGrabRotateConstraints
        {
            public FloatConstraint MinAngle;
            public FloatConstraint MaxAngle;
        }

        [SerializeField]
        private OneGrabRotateConstraints _constraints;

        public OneGrabRotateConstraints Constraints
        {
            get { return _constraints; }
            set { _constraints = value; }
        }

        private float _relativeAngle = 0.0f;
        private float _constrainedRelativeAngle = 0.0f;

        private IGrabbable _grabbable;

        private Pose _initialPose;
        private Pose _previousGrabPose;

        // [CHANGED] 0~1 사이의 슬라이더로 노브의 회전 정도를 표시하며, 외부에서 참조할 수 있도록 public getter 제공
        [Header("Knob Settings")]

        [SerializeField] private Slider targetSlider;

        [Range(0f, 1f)]
        [SerializeField]
        private float _currentValue = 0.0f; // Inspector에서 설정한 초기 값

        // [CHANGED] 회전 민감도(Twist Sensitivity)를 조절하기 위한 변수
        [SerializeField]
        private float _twistSensitivity = 1.0f;

        // [CHANGED] Inspector에서 설정한 초기 값에 따른 회전 적용을 위한 기본 상태 변수
        private Quaternion _baseRotation;
        private Vector3 _basePosition;
        private bool _hasInitializedBaseState = false;

        private void Awake()
        {
            // [CHANGED] 게임 시작 시 현재 오브젝트의 회전과 위치를 기본 상태로 저장
            _baseRotation = transform.rotation;
            _basePosition = transform.position;
            _hasInitializedBaseState = true;

            // [CHANGED] Awake에서 슬라이더 값(_currentValue)에 따라 오브젝트의 회전과 위치를 업데이트하여 초기 value 반영
            UpdateRotationFromValue();

            // [ADDED] 슬라이더가 할당되어 있다면 초기 _currentValue를 반영하여 슬라이더의 value 업데이트
            if (targetSlider != null)
            {
                targetSlider.value = _currentValue;
            }
        }

        // [CHANGED] Inspector에서 변경한 슬라이더 값에 따라 오브젝트의 회전과 위치를 업데이트하는 메서드
        private void UpdateRotationFromValue()
        {
            // _constraints에 설정된 최소, 최대 각도를 사용하여 원하는 각도를 계산
            float minAngle = _constraints.MinAngle.Constrain ? _constraints.MinAngle.Value : 0f;
            float maxAngle = _constraints.MaxAngle.Constrain ? _constraints.MaxAngle.Value : 360f;
            float desiredAngle = Mathf.Lerp(minAngle, maxAngle, _currentValue);

            // 회전 기준 피벗. 지정되지 않았다면 현재 오브젝트를 기준으로 함.
            Transform pivot = _pivotTransform != null ? _pivotTransform : transform;
            Vector3 rotationAxis = pivot.TransformDirection(Vector3.up);

            // [CHANGED] 기본 상태(_basePosition)에서 피벗을 기준으로 한 방향 벡터 계산
            Vector3 initialDir = _basePosition - pivot.position;
            // 슬라이더 값에 따른 각도로 해당 벡터를 회전시킴
            Vector3 rotatedDir = Quaternion.AngleAxis(desiredAngle, rotationAxis) * initialDir;
            Vector3 newPosition = pivot.position + rotatedDir;

            // [CHANGED] 기본 회전(_baseRotation)에 원하는 각도만큼 회전을 추가하여 새로운 회전 계산
            Quaternion newRotation = Quaternion.AngleAxis(desiredAngle, rotationAxis) * _baseRotation;

            // [CHANGED] 계산된 위치와 회전을 Scene의 오브젝트에 적용
            transform.position = newPosition;
            transform.rotation = newRotation;
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
