using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public int damage = 10;
    [SerializeField] private Animator animator;
    [SerializeField] private string attackTrigger = "Attack";
    [SerializeField] private string deathTrigger = "Death";
    private HealthBase _health;

    private void Awake()
    {
        _health = GetComponent<HealthBase>();
        if (_health != null)
        {
            _health.OnKill += OnDeath;
        }
    }

    private void OnDeath()
    {
        _health.OnKill -= OnDeath;
        animator.SetTrigger(deathTrigger);
    }

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
