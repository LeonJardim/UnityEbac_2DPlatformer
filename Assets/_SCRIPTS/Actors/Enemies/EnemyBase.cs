using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public int damage = 10;
    [SerializeField] private Animator animator;
    [SerializeField] private string attackTrigger = "Attack";
    [SerializeField] private string deathTrigger = "Death";

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var health = collision.gameObject.GetComponent<HealthBase>();

        if (health != null)
        {
            health.TakeDamage(damage);
            animator.SetTrigger(attackTrigger);
        }
    }
}
