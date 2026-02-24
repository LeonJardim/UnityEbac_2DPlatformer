using UnityEngine;

public class Collectable : MonoBehaviour
{
    public string tagToCompare = "Player";
    public ParticleSystem particles;
    public GameObject spriteRenderer;
    public float timeToDestroy;
    private bool _collected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(tagToCompare))
        {
            if (_collected) return;
            Collect();
        }
    }

    protected virtual void Collect()
    {
        if (_collected) return;
        _collected = true;
        OnCollect();
        Invoke("DestroyMe", timeToDestroy);
    }
    protected virtual void OnCollect()
    {
        if (particles != null) particles.Play();
        if (spriteRenderer != null) spriteRenderer.SetActive(false);
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }
}
