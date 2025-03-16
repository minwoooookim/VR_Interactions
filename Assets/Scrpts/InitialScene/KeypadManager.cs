using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Initial_Keypad
{
    public class KeypadManager : MonoBehaviour
    {
        public static KeypadManager instance;
        public GameObject deleteButton;
        
        private Image deleteButtonImage;

        public TMP_InputField inputField;

        [Header("Colors")]
        public Color normalColor = Color.white;
        public Color pressedColor = Color.gray;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            deleteButtonImage = deleteButton.gameObject.GetComponent<Image>();
        }

        /// <summary>
        /// �Ʒ� �ڵ�� �� ��ư�� Event Trigger�� ����Ǿ�� ��
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
    }
}