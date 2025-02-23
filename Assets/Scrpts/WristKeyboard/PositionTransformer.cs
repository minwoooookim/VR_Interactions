using UnityEngine;

namespace WristKeyboard
{
    public class PositionTransformer : MonoBehaviour
    {
        void Update()
        {
            ChangePosition();
        }

        // �θ� ������ ���� ���� Ű����(��Ʈ�ѷ�) ����("Right Side" �Ǵ� "Left Side")�� ��ȯ
        private string WhichSide()
        {
            Transform sideTransform = transform.parent.parent;
            return sideTransform.gameObject.name;
        }

        public void ChangePosition()
        {
            // [����] WristKeyboardManager�� �����ε�� GetZInput(string side)�� ȣ���Ͽ�
            // ���� ����� ��Ʈ�ѷ� ȸ���� ����� Z������ �޾ƿ�.
            float angle = WristKeyboardManager.instance.GetZInput(WhichSide());

            // ������ ���� �÷��� ��������
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
