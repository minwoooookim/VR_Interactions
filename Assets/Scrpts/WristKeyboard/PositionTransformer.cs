using UnityEngine;

namespace WristKeyboard
{
    public class PositionTransformer : MonoBehaviour
    {
        void Update()
        {
            ChangePosition();
        }

        // 부모 계층을 통해 현재 키보드(컨트롤러) 측면("Right Side" 또는 "Left Side")을 반환
        private string WhichSide()
        {
            Transform sideTransform = transform.parent.parent;
            return sideTransform.gameObject.name;
        }

        public void ChangePosition()
        {
            // [변경] WristKeyboardManager의 오버로드된 GetZInput(string side)를 호출하여
            // 현재 저장된 컨트롤러 회전값 기반의 Z각도를 받아옴.
            float angle = WristKeyboardManager.instance.GetZInput(WhichSide());

            // 각도에 따라 플래그 가져오기
            WristKeyboardManager.AngleFlag angleFlag = WristKeyboardManager.AngleUtility.GetAngleFlag(angle);

            float position = 0f;
            switch (angleFlag)
            {
                case WristKeyboardManager.AngleFlag.LeftMost:
                    position = -0.2f;
                    break;
                case WristKeyboardManager.AngleFlag.NearLeft:
                    position = -0.1f;
                    break;
                case WristKeyboardManager.AngleFlag.Center:
                    position = 0f;
                    break;
                case WristKeyboardManager.AngleFlag.NearRight:
                    position = 0.1f;
                    break;
                case WristKeyboardManager.AngleFlag.RightMost:
                    position = 0.2f;
                    break;
            }

            transform.localPosition = new Vector3(0, position, 0);
        }
    }
}
