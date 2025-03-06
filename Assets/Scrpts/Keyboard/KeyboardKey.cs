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

            //Shift 기능은 실험에 필요하지 않으므로 구현중 지움

            // 숫자키의 경우 Shift 시 특수문자 처리 
            //string numbers = "1234567890";
            //if (numbers.Contains(keyLabel.text))
            //{
            //    shiftCharacter = GetShiftCharacter();
            //}

            // 초기 라벨 업데이트: KeyboardManager의 isShifted 값을 반영
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
        /// 아래 코드는 제대로 동작하지 않음
        /// KeyboardManager의 isShifted 값이 변할때마다 호출되어야 함.
        /// 만약 Shift 기능이 필요하다면 KeyboardManager에 구현해야 함.
        /// </summary>
        //public void UpdateKeyLabel()
        //{
        //    keyLabel.text = KeyboardManager.instance.isShifted ? shiftCharacter : character;
        //}

        /// <summary>
        /// 누르는 순간 색상 변경 & 키 입력
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
        /// 손을 뗐을 때 색상 복귀
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
