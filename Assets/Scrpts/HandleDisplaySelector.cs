using UnityEngine;
using TMPro;

public enum HandleMode
{
    Audio,
    Brightness,
    Video
}

public class HandleDisplaySelector : MonoBehaviour
{
    [SerializeField] private HandleMode handleMode = HandleMode.Brightness;
    [SerializeField] private TextMeshProUGUI displayText;

    public void UpdateDisplayText(float sliderValue)
    {
        switch (handleMode)
        {
            case HandleMode.Brightness:
                int brightnessValue = Mathf.RoundToInt(sliderValue * 10f);
                if (displayText != null)
                    displayText.text = brightnessValue.ToString();
                break;

            case HandleMode.Audio:
                int audioValue = Mathf.RoundToInt(sliderValue * 30f);
                if (displayText != null)
                    displayText.text = audioValue.ToString();
                break;

        }
    }
}
