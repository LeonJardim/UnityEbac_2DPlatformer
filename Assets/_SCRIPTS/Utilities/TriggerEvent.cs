using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    [SerializeField] private string tagToCompare = "Player";
    public UnityEvent onTagEntered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(tagToCompare))
        {
            onTagEntered.Invoke();
        }
    }
}
