using UnityEngine;
using Unity.VisualScripting;
using UnityEditor;

namespace WristKeyboard
{
    public class KeyPrinter : MonoBehaviour
    {
        public static KeyPrinter instance;

        public string character1;
        public string shiftCharacter1;
        public string character2;
        public string shiftCharacter2;
        public string character3;
        public string shiftCharacter3;
        public string character4;
        public string shiftCharacter4;
        public string character5;
        public string shiftCharacter5;

        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        public void PrintKeyBasedOnAngle(float Angle)
        {
            // [����] AngleUtility�� AngleFlag�� WristKeyboardManager�� ���� ����
            WristKeyboardManager.AngleFlag angleFlag = WristKeyboardManager.AngleUtility.GetAngleFlag(Angle);

            // �÷��׿� ���� �ٸ� Ű�� ���
            switch (angleFlag)
            {
                case WristKeyboardManager.AngleFlag.LeftMost:
                    WristKeyboardManager.instance.inputField.text += character1;
                    break;
                case WristKeyboardManager.AngleFlag.NearLeft:
                    WristKeyboardManager.instance.inputField.text += character2;
                    break;
                case WristKeyboardManager.AngleFlag.Center:
                    WristKeyboardManager.instance.inputField.text += character3;
                    break;
                case WristKeyboardManager.AngleFlag.NearRight:
                    WristKeyboardManager.instance.inputField.text += character4;
                    break;
                case WristKeyboardManager.AngleFlag.RightMost:
                    WristKeyboardManager.instance.inputField.text += character5;
                    break;
            }

            Debug.Log("PrintKeyBasedOnAngle executed");
        }
    }
}
