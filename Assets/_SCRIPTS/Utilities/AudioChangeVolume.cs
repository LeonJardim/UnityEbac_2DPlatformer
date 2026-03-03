using UnityEngine;
using UnityEngine.Audio;

public class AudioChangeVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer group;
    [SerializeField] private string floatParam = "MyExposedParam";

    public void ChangeValue(float f)
    {
        group.SetFloat(floatParam, f);
    }
}
