using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public void Pause()
    {
        Time.timeScale = 0.0f;
    }

    public void UnPause()
    {
        Time.timeScale = 1.0f;
    }
}
