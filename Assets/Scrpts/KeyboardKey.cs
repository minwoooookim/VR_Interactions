using TMPro;
using UnityEngine;
using UnityEngine.EventSystems; // IPointerDownHandler, IPointerUpHandler
using UnityEngine.UI;

namespace QWERTY_Keyboard
{
    public class KeyboardKey : MonoBehaviour, IPointerDownHandler, IPointerUpHandler 
    {
        public string character;
        public string shiftCharacter;
        public TextMeshProUGUI keyLabel;

        [Header("Colors")]
        public Color normalColor = Color.white;
        public Color pressedColor = Color.gray;

        private Image keyImage;

        private void Start()
        {
            keyImage = GetComponent<Image>();
            character = keyLabel.text;
            shiftCharacter = keyLabel.text.ToUpper();

            //Shift ����� ���迡 �ʿ����� �����Ƿ� ������ ����

            // ����Ű�� ��� Shift �� Ư������ ó�� 
            //string numbers = "1234567890";
            //if (numbers.Contains(keyLabel.text))
            //{
            //    shiftCharacter = GetShiftCharacter();
            //}

            // �ʱ� �� ������Ʈ: KeyboardManager�� isShifted ���� �ݿ�
            //UpdateKeyLabel();

        }

        private string GetShiftCharacter()
        {
            switch (keyLabel.text)
            {
                case "1": return "!";
                case "2": return "@";
                case "3": return "#";
                case "4": return "$";
                case "5": return "%";
                case "6": return "^";
                case "7": return "&";
                case "8": return "*";
                case "9": return "(";
                case "0": return ")";
                default: return string.Empty;
            }
        }

        /// <summary>
        /// �Ʒ� �ڵ�� ����� �������� ����
        /// KeyboardManager�� isShifted ���� ���Ҷ����� ȣ��Ǿ�� ��.
        /// ���� Shift ����� �ʿ��ϴٸ� KeyboardManager�� �����ؾ� ��.
        /// </summary>
        //public void UpdateKeyLabel()
        //{
        //    keyLabel.text = KeyboardManager.instance.isShifted ? shiftCharacter : character;
        //}

        /// <summary>
        /// ������ ���� ���� ���� & Ű �Է�
        /// </summary>
        public void OnPointerDown(PointerEventData eventData)
        {
            if (keyImage != null)
            {
                keyImage.color = pressedColor;
            }

            TypeKey();
        }

        /// <summary>
        /// ���� ���� �� ���� ����
        /// </summary>
        public void OnPointerUp(PointerEventData eventData)
        {
            if (keyImage != null)
            {
                keyImage.color = normalColor;
            }
        }

        private void TypeKey()
        {
            if (KeyboardManager.instance.isShifted)
            {
                KeyboardManager.instance.inputField.text += shiftCharacter;
            }
            else
            {
                KeyboardManager.instance.inputField.text += character;
            }
        }
    }
}
