using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace QWERTY_Keyboard
{
    public class KeyboardManager : MonoBehaviour 
    {
        public static KeyboardManager instance;
        //public GameObject shiftButton; //Shift 기능은 실험에 필요하지 않으므로 구현중 지움
        public GameObject deleteButton;
        public GameObject spaceButton;
        //private Image shiftButtonImage;
        private Image deleteButtonImage;
        private Image spaceButtonImage;

        public TMP_InputField inputField;

        [Header("Colors")]
        public Color normalColor = Color.white;
        public Color pressedColor = Color.gray;

        public bool isShifted = false;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            //shiftButtonImage = shiftButton.gameObject.GetComponent<Image>();
            deleteButtonImage = deleteButton.gameObject.GetComponent<Image>();
            spaceButtonImage = spaceButton.gameObject.GetComponent<Image>();
        }

        //public void Shifted()
        //{
        //    isShifted = !isShifted;

        //    if (isShifted)
        //    {
        //        shiftButtonImage.color = Color.yellow;
        //    }
        //    else
        //    {
        //        shiftButtonImage.color = Color.white;
        //    }
        //}


        /// <summary>
        /// 아래 코드는 각 버튼의 Event Trigger에 연결되어야 함
        /// </summary>
        public void DeleteOnPointerDown()
        {
            if (inputField.text.Length > 0)
            {
                inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
            }
            deleteButtonImage.color = pressedColor;
        }

        public void DeleteOnPointerUp()
        {
            deleteButtonImage.color = normalColor;
        }

        public void SpaceOnPointerDown()
        {
            inputField.text += " ";
            spaceButtonImage.color = pressedColor;
        }

        public void SpaceOnPointerUp()
        {
            spaceButtonImage.color = normalColor;
        }
    }
}
