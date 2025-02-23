using UnityEngine;
using TMPro;

namespace WristKeyboard
{
    public class KeyLabelHandler : MonoBehaviour
    {
        public TextMeshProUGUI keyLabel;
        private KeyPrinter keyPrinter;

        private void Awake()
        {
            // �θ��� �θ��� KeyPrinter
            keyPrinter = transform.parent?.parent.GetComponent<KeyPrinter>();

            // KeyPrinter�� ��� �ʵ带 �˻�
            var fields = keyPrinter.GetType().GetFields();

            foreach (var field in fields)
            {
                if (field.Name == this.gameObject.name)
                {
                    keyLabel.text = field.GetValue(keyPrinter).ToString();
                    break;
                }
            }
        }
    }
}
