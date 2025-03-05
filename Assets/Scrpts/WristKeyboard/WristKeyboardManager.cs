using UnityEngine;
using TMPro;
// �ʿ��: using Oculus; using OVR;

namespace WristKeyboard
{
    public class WristKeyboardManager : MonoBehaviour
    {
        public static WristKeyboardManager instance;
        public TMP_InputField inputField;

        // XRInteraction Toolkit ���� �ʵ� ���� (OVRInput���� ��ü)

        // ���� �� ��Ʈ�ѷ��� ȸ������ ���� (PositionTransformer���� ���)
        public Quaternion currentRightRotation { get; private set; }
        public Quaternion currentLeftRotation { get; private set; }

        private GameObject currentKeyIndicator_Right = null;
        private GameObject currentKeyIndicator_Left = null;

        private bool isRightPrintKeyCalled = false;
        private bool isLeftPrintKeyCalled = false;

        private bool isDeleteCalled = false;
        private bool isSpaceCalled = false;

        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            FindKeyIndicator("KeyBars_Y", "Right Side").SetActive(false);
            FindKeyIndicator("KeyBars_H", "Right Side").SetActive(false);
            FindKeyIndicator("KeyBars_N", "Right Side").SetActive(false);
            FindKeyIndicator("KeyBars_T", "Left Side").SetActive(false);
            FindKeyIndicator("KeyBars_G", "Left Side").SetActive(false);
            FindKeyIndicator("KeyBars_B", "Left Side").SetActive(false);
        }

        private void Update()
        {
            // [�߰�] OVRInput�� ���� ������ �Է°� ��������
            Quaternion rightControllerRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
            float rightTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
            float rightGripValue = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch);

            bool isRightTriggerPressed = rightTriggerValue > 0.1f;
            bool isRightGripPressed = rightGripValue > 0.1f;

            // [�߰�] OVRInput�� ���� �޼� �Է°� ��������
            Quaternion leftControllerRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
            float leftTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
            float leftGripValue = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch);

            bool isLeftTriggerPressed = leftTriggerValue > 0.1f;
            bool isLeftGripPressed = leftGripValue > 0.1f;

            // [�߰�] �ֽ� ȸ���� ���� (PositionTransformer���� ���)
            currentRightRotation = rightControllerRotation;
            currentLeftRotation = leftControllerRotation;

            // ���� RightHand(), LeftHand()�� �Ű������� �Է°��� �����ϵ��� ����
            RightHand(rightControllerRotation, isRightTriggerPressed, isRightGripPressed);
            LeftHand(leftControllerRotation, isLeftTriggerPressed, isLeftGripPressed);

            Debug.Log($"Right Trigger Value: {rightTriggerValue}");
            Debug.Log("������ ���� ������: " + AdjustRotation("Right Side", rightControllerRotation).eulerAngles);
            Debug.Log("������ ������ ����: " + rightControllerRotation.eulerAngles);
        }

        /* ��� ��� �κ� (�Է°��� �Ű������� ����) */
        private void RightHand(Quaternion rightRotation, bool isTriggerPressed, bool isGripPressed)
        {
            var rightControllerRotation_Z = GetZInput("Right Side", rightRotation);
            var rightControllerRotation_X = GetXInput("Right Side", rightRotation);

            if (!isTriggerPressed)
            {
                SelectBar_Right(rightControllerRotation_X);
                isRightPrintKeyCalled = false;
            }
            else if (isTriggerPressed && !isRightPrintKeyCalled)
            {
                ExecutePrintKey_Right(rightControllerRotation_Z);
                isRightPrintKeyCalled = true;
            }

            // ����� ��� (�׸� �Է�)
            if (isGripPressed && !isDeleteCalled)
            {
                DeleteOnce();
                isDeleteCalled = true;
            }
            else if (!isGripPressed)
            {
                isDeleteCalled = false;
            }
        }

        private void LeftHand(Quaternion leftRotation, bool isTriggerPressed, bool isGripPressed)
        {
            var leftControllerRotation_Z = GetZInput("Left Side", leftRotation);
            var leftControllerRotation_X = GetXInput("Left Side", leftRotation);

            if (!isTriggerPressed)
            {
                SelectBar_Left(leftControllerRotation_X);
                isLeftPrintKeyCalled = false;
            }
            else if (isTriggerPressed && !isLeftPrintKeyCalled)
            {
                ExecutePrintKey_Left(leftControllerRotation_Z);
                isLeftPrintKeyCalled = true;
            }

            // �� ĭ ���� ��� (�޼� �׸� �Է�)
            if (isGripPressed && !isSpaceCalled)
            {
                inputField.text += " ";
                isSpaceCalled = true;
            }
            else if (!isGripPressed)
            {
                isSpaceCalled = false;
            }
        }

        /* Ű��(Indicator) ���� �� Ű ��� */
        private void SelectBar_Right(float ControllerRotation_X)
        {
            GameObject targetKeyIndicator = null;
            WhichBar selectedBar = SelectBar(ControllerRotation_X);

            switch (selectedBar)
            {
                case WhichBar.Top:
                    targetKeyIndicator = FindKeyIndicator("KeyBars_Y", "Right Side");
                    break;
                case WhichBar.Center:
                    targetKeyIndicator = FindKeyIndicator("KeyBars_H", "Right Side");
                    break;
                case WhichBar.Bottom:
                    targetKeyIndicator = FindKeyIndicator("KeyBars_N", "Right Side");
                    break;
            }

            if (targetKeyIndicator != null && targetKeyIndicator != currentKeyIndicator_Right)
            {
                HideCurrentKeyIndicator_Right();
                currentKeyIndicator_Right = targetKeyIndicator;
                currentKeyIndicator_Right.SetActive(true);
            }
        }

        private void SelectBar_Left(float ControllerRotation_X)
        {
            GameObject targetKeyIndicator = null;
            WhichBar selectedBar = SelectBar(ControllerRotation_X);

            switch (selectedBar)
            {
                case WhichBar.Top:
                    targetKeyIndicator = FindKeyIndicator("KeyBars_T", "Left Side");
                    break;
                case WhichBar.Center:
                    targetKeyIndicator = FindKeyIndicator("KeyBars_G", "Left Side");
                    break;
                case WhichBar.Bottom:
                    targetKeyIndicator = FindKeyIndicator("KeyBars_B", "Left Side");
                    break;
            }

            if (targetKeyIndicator != null && targetKeyIndicator != currentKeyIndicator_Left)
            {
                HideCurrentKeyIndicator_Left();
                currentKeyIndicator_Left = targetKeyIndicator;
                currentKeyIndicator_Left.SetActive(true);
            }
        }

        private GameObject FindKeyIndicator(string parentName, string side)
        {
            return transform.Find(side + "/" + parentName + "/KeyIndicator").gameObject;
        }

        private void HideCurrentKeyIndicator_Right()
        {
            if (currentKeyIndicator_Right != null)
            {
                currentKeyIndicator_Right.SetActive(false);
                currentKeyIndicator_Right = null;
            }
        }

        private void HideCurrentKeyIndicator_Left()
        {
            if (currentKeyIndicator_Left != null)
            {
                currentKeyIndicator_Left.SetActive(false);
                currentKeyIndicator_Left = null;
            }
        }

        private void ExecutePrintKey_Right(float zAngle)
        {
            Transform parentTransform_Right = currentKeyIndicator_Right.transform.parent;
            var component_Right = parentTransform_Right.GetComponent<KeyPrinter>();
            component_Right.PrintKeyBasedOnAngle(zAngle);
        }

        private void ExecutePrintKey_Left(float zAngle)
        {
            Transform parentTransform_Left = currentKeyIndicator_Left.transform.parent;
            var component_Left = parentTransform_Left.GetComponent<KeyPrinter>();
            component_Left.PrintKeyBasedOnAngle(zAngle);
        }

        private void DeleteOnce()
        {
            int length = inputField.text.Length - 1;
            if (length < 0) return;
            inputField.text = inputField.text.Substring(0, length);
        }

        /* ȸ�� ���� ���� (�Է¹��� Quaternion�� ������ ����) */
        private Quaternion AdjustRotation(string side, Quaternion originalRotation)
        {
            if (side == "Right Side")
            {
                return originalRotation * Quaternion.Euler(0f, -20f, 0f);
            }
            else if (side == "Left Side")
            {
                return originalRotation * Quaternion.Euler(0f, 20f, 0f);
            }
            else
            {
                return Quaternion.identity;
            }
        }

        // ���� GetZInput, GetXInput�� �Ű������� rotation�� ����
        public float GetZInput(string side, Quaternion rotation)
        {
            Quaternion adjustedRotation = AdjustRotation(side, rotation);
            return AdjustZAngle(adjustedRotation.eulerAngles.z);
        }

        public float GetXInput(string side, Quaternion rotation)
        {
            Quaternion adjustedRotation = AdjustRotation(side, rotation);
            return adjustedRotation.eulerAngles.x;
        }

        // [�߰�] ��Ʈ�ѷ��� �ֽ� ȸ������ ����Ͽ� ���� ���� ���̵� ȣ���� �� �ֵ��� �����ε���.
        public float GetZInput(string side)
        {
            if (side == "Right Side")
                return GetZInput(side, currentRightRotation);
            else if (side == "Left Side")
                return GetZInput(side, currentLeftRotation);
            return 0f;
        }

        private float AdjustZAngle(float inputZAngle)
        {
            if ((inputZAngle >= 0 && inputZAngle <= 90) || (inputZAngle >= 270 && inputZAngle <= 360))
            {
                float adjustedZAngle;
                float sensitivity = 1.3f; // Z���� �ΰ���

                if (inputZAngle <= 90)
                {
                    adjustedZAngle = inputZAngle * sensitivity;
                    adjustedZAngle = Mathf.Clamp(adjustedZAngle, 0, 90);
                }
                else
                {
                    adjustedZAngle = 360 - (360 - inputZAngle) * sensitivity;
                    adjustedZAngle = Mathf.Clamp(adjustedZAngle, 270, 360);
                }
                return adjustedZAngle;
            }
            else
            {
                return inputZAngle;
            }
        }

        private WhichBar SelectBar(float ControllerRotation_X)
        {
            if (ControllerRotation_X >= 180 && ControllerRotation_X < 345)
            {
                return WhichBar.Top;
            }
            else if (ControllerRotation_X >= 345 || ControllerRotation_X < 15)
            {
                return WhichBar.Center;
            }
            else if (ControllerRotation_X >= 15 && ControllerRotation_X < 180)
            {
                return WhichBar.Bottom;
            }
            else
            {
                return WhichBar.Center;
            }
        }

        public enum WhichBar
        {
            Top,
            Center,
            Bottom
        }

        public enum AngleFlag
        {
            LeftMost,
            NearLeft,
            Center,
            NearRight,
            RightMost
        }

        public static class AngleUtility
        {
            public static AngleFlag GetAngleFlag(float Angle)
            {
                if (Angle >= 54 && Angle < 180)
                {
                    return AngleFlag.LeftMost;
                }
                else if (Angle >= 18 && Angle < 54)
                {
                    return AngleFlag.NearLeft;
                }
                else if (Angle >= 342 || Angle < 18)
                {
                    return AngleFlag.Center;
                }
                else if (Angle >= 306 && Angle < 342)
                {
                    return AngleFlag.NearRight;
                }
                else if (Angle >= 180 && Angle < 306)
                {
                    return AngleFlag.RightMost;
                }
                return AngleFlag.Center;
            }
        }
    }
}
