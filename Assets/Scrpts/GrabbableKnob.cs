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
    /// Inspector���� ������ �����̴�(_currentValue) ���� �ʱⰪ���� �ݿ��ǵ��� Awake()���� �⺻ ���¸� ������ ��,
    /// UpdateRotationFromValue()�� ȣ���Ͽ� ������Ʈ�� ȸ���� ��ġ�� �����մϴ�.
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

        // [CHANGED] 0~1 ������ �����̴��� ����� ȸ�� ������ ǥ���ϸ�, �ܺο��� ������ �� �ֵ��� public getter ����
        [Header("Knob Settings")]

        [SerializeField] private Slider targetSlider;

        [Range(0f, 1f)]
        [SerializeField]
        private float _currentValue = 0.0f; // Inspector���� ������ �ʱ� ��

        // [CHANGED] ȸ�� �ΰ���(Twist Sensitivity)�� �����ϱ� ���� ����
        [SerializeField]
        private float _twistSensitivity = 1.0f;

        // [CHANGED] Inspector���� ������ �ʱ� ���� ���� ȸ�� ������ ���� �⺻ ���� ����
        private Quaternion _baseRotation;
        private Vector3 _basePosition;
        private bool _hasInitializedBaseState = false;

        private void Awake()
        {
            // [CHANGED] ���� ���� �� ���� ������Ʈ�� ȸ���� ��ġ�� �⺻ ���·� ����
            _baseRotation = transform.rotation;
            _basePosition = transform.position;
            _hasInitializedBaseState = true;

            // [CHANGED] Awake���� �����̴� ��(_currentValue)�� ���� ������Ʈ�� ȸ���� ��ġ�� ������Ʈ�Ͽ� �ʱ� value �ݿ�
            UpdateRotationFromValue();

            // [ADDED] �����̴��� �Ҵ�Ǿ� �ִٸ� �ʱ� _currentValue�� �ݿ��Ͽ� �����̴��� value ������Ʈ
            if (targetSlider != null)
            {
                targetSlider.value = _currentValue;
            }
        }

        // [CHANGED] Inspector���� ������ �����̴� ���� ���� ������Ʈ�� ȸ���� ��ġ�� ������Ʈ�ϴ� �޼���
        private void UpdateRotationFromValue()
        {
            // _constraints�� ������ �ּ�, �ִ� ������ ����Ͽ� ���ϴ� ������ ���
            float minAngle = _constraints.MinAngle.Constrain ? _constraints.MinAngle.Value : 0f;
            float maxAngle = _constraints.MaxAngle.Constrain ? _constraints.MaxAngle.Value : 360f;
            float desiredAngle = Mathf.Lerp(minAngle, maxAngle, _currentValue);

            // ȸ�� ���� �ǹ�. �������� �ʾҴٸ� ���� ������Ʈ�� �������� ��.
            Transform pivot = _pivotTransform != null ? _pivotTransform : transform;
            Vector3 rotationAxis = pivot.TransformDirection(Vector3.up);

            // [CHANGED] �⺻ ����(_basePosition)���� �ǹ��� �������� �� ���� ���� ���
            Vector3 initialDir = _basePosition - pivot.position;
            // �����̴� ���� ���� ������ �ش� ���͸� ȸ����Ŵ
            Vector3 rotatedDir = Quaternion.AngleAxis(desiredAngle, rotationAxis) * initialDir;
            Vector3 newPosition = pivot.position + rotatedDir;

            // [CHANGED] �⺻ ȸ��(_baseRotation)�� ���ϴ� ������ŭ ȸ���� �߰��Ͽ� ���ο� ȸ�� ���
            Quaternion newRotation = Quaternion.AngleAxis(desiredAngle, rotationAxis) * _baseRotation;

            // [CHANGED] ���� ��ġ�� ȸ���� Scene�� ������Ʈ�� ����
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

            // [CHANGED] ȸ�� �ΰ���(TwistSensitivity)�� �����ϱ� ���� rawAngleDelta�� ����
            var rawAngleDelta = Vector3.SignedAngle(targetPose.up, previousPose.up, _initialPose.forward);
            var angleDelta = rawAngleDelta * _twistSensitivity; // �ΰ��� ����

            float previousAngle = _constrainedRelativeAngle;

            _relativeAngle += angleDelta;
            _constrainedRelativeAngle = _relativeAngle;

            // Min/Max ���� ������ ������ ����
            if (_constraints.MinAngle.Constrain)
            {
                _constrainedRelativeAngle = Mathf.Max(_constrainedRelativeAngle, _constraints.MinAngle.Value);
            }
            if (_constraints.MaxAngle.Constrain)
            {
                _constrainedRelativeAngle = Mathf.Min(_constrainedRelativeAngle, _constraints.MaxAngle.Value);
            }

            // ���� �� ������ ����Ǵ� angleDelta�� �ٽ� ���
            angleDelta = _constrainedRelativeAngle - previousAngle;

            // ȸ�� ����
            targetTransform.RotateAround(pivot.position, rotationAxis, angleDelta);

            // [CHANGED] 0~1 �����̴� ��(_currentValue)�� ������Ʈ
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

            // [ADDED] �����̴��� �Ҵ�Ǿ� �ִٸ� ������Ʈ�� _currentValue�� �ݿ��Ͽ� �����̴��� value ������Ʈ
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
