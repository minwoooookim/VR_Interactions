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
            // 부모의 부모의 KeyPrinter
            keyPrinter = transform.parent?.parent.GetComponent<KeyPrinter>();

            // KeyPrinter의 모든 필드를 검사
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
