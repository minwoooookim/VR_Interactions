using UnityEngine;
using TMPro;
using UnityEngine.Video;
using System.Runtime.InteropServices;

public enum HandleMode
{
    Audio,
    Brightness,
    Video
}

public class HandleDisplaySelector : MonoBehaviour
{
    [SerializeField] private HandleMode handleMode = HandleMode.Audio;
    [SerializeField] private VideoPlayer videoPlayer = null;
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

            case HandleMode.Video:
                double currentTime = sliderValue * videoPlayer.length;
                if (displayText != null)
                    displayText.text = FormatTime(currentTime);
                break;
        }
    }

    private string FormatTime(double time)
    {
        int minutes = Mathf.FloorToInt((float)time / 60f);
        int seconds = Mathf.FloorToInt((float)time % 60f);
        return $"{minutes:D1}:{seconds:D2}";
    }
}
