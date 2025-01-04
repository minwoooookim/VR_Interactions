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
        public Button shiftButton;
        public Button deleteButton;
        public Button spaceButton;
        private Image shiftButtonImage;

        public TMP_InputField inputField;

        private bool isShifted = false;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            shiftButton.onClick.AddListener(Shifted);
            shiftButtonImage = shiftButton.gameObject.GetComponent<Image>();
            deleteButton.onClick.AddListener(Delete);
            spaceButton.onClick.AddListener(Space);
        }

        private void Shifted()
        {
            isShifted = !isShifted;

            if (isShifted)
            {
                shiftButtonImage.color = Color.yellow;
            }
            else
            {
                shiftButtonImage.color = Color.white;
            }
        }

        private void Delete()
        {
            if (inputField.text.Length > 0)
            {
                inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
            }
        }

        private void Space()
        {
            inputField.text += " ";
        }
    }
}
