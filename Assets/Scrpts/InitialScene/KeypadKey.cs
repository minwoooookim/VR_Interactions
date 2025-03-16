using TMPro;
using UnityEngine;
using UnityEngine.EventSystems; // IPointerDownHandler, IPointerUpHandler
using UnityEngine.UI;

namespace Initial_Keypad
{
    public class KeypadKey : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public string character;
        public TextMeshProUGUI keyLabel;

        [Header("Colors")]
        public Color normalColor = Color.white;
        public Color pressedColor = Color.gray;

        private Image keyImage;

        private void Start()
        {
            keyImage = GetComponent<Image>();
            character = keyLabel.text;
        }

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
            KeypadManager.instance.inputField.text += character;
        }
    }
}
