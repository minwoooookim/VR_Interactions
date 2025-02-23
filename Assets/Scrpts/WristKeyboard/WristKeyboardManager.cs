using UnityEngine;
using TMPro;
// 필요시: using Oculus; using OVR;

namespace WristKeyboard
{
    public class WristKeyboardManager : MonoBehaviour
    {
        public static WristKeyboardManager instance;
        public TMP_InputField inputField;

        // XRInteraction Toolkit 전용 필드 제거 (OVRInput으로 대체)

        // 현재 각 컨트롤러의 회전값을 저장 (PositionTransformer에서 사용)
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
            // [추가] OVRInput을 통해 오른손 입력값 가져오기
            Quaternion rightControllerRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
            float rightTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
            float rightGripValue = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch);

            bool isRightTriggerPressed = rightTriggerValue > 0.1f;
            bool isRightGripPressed = rightGripValue > 0.1f;

            // [추가] OVRInput을 통해 왼손 입력값 가져오기
            Quaternion leftControllerRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
            float leftTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
            float leftGripValue = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch);

            bool isLeftTriggerPressed = leftTriggerValue > 0.1f;
            bool isLeftGripPressed = leftGripValue > 0.1f;

            // [추가] 최신 회전값 저장 (PositionTransformer에서 사용)
            currentRightRotation = rightControllerRotation;
            currentLeftRotation = leftControllerRotation;

            // 기존 RightHand(), LeftHand()를 매개변수로 입력값을 전달하도록 수정
            RightHand(rightControllerRotation, isRightTriggerPressed, isRightGripPressed);
            LeftHand(leftControllerRotation, isLeftTriggerPressed, isLeftGripPressed);

            Debug.Log($"Right Trigger Value: {rightTriggerValue}");
            Debug.Log("오프셋 적용 오른쪽: " + AdjustRotation("Right Side", rightControllerRotation).eulerAngles);
            Debug.Log("오프셋 미적용 우측: " + rightControllerRotation.eulerAngles);
        }

        /* 양손 기능 부분 (입력값을 매개변수로 받음) */
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

            // 지우기 기능 (그립 입력)
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

            // 한 칸 띄우기 기능 (왼손 그립 입력)
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

        /* 키바(Indicator) 선택 및 키 출력 */
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

        /* 회전 각도 조절 (입력받은 Quaternion에 오프셋 적용) */
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

        // 기존 GetZInput, GetXInput은 매개변수로 rotation을 받음
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

        // [추가] 컨트롤러의 최신 회전값을 사용하여 별도 인자 없이도 호출할 수 있도록 오버로드함.
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
                float sensitivity = 1.3f; // Z각도 민감도

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
