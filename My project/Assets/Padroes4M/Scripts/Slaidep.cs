using UnityEngine;
using UnityEngine.UI;

public class Slaidep : MonoBehaviour
{
    private AudioManager audioManager;
    private Slider slider;

    private void Start()
    {
        audioManager = AudioManager.Instance;
        slider = GetComponent<Slider>();

        if (slider != null && audioManager != null)
        {
            slider.value = audioManager.Volume;
            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }
    }

    private void OnSliderValueChanged(float value)
    {
        if (audioManager != null)
        {
            audioManager.Volume = value;
        }
    }
}