using UnityEngine;

public class DestroyMe : MonoBehaviour
{
    public GameObject obj;

    public void KillMe()
    {
        Destroy(obj);
    }
}
