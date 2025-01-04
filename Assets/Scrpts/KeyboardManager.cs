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
    }
}
