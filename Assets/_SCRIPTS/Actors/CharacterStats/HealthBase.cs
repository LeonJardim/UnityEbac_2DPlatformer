using UnityEngine;

public class HealthBase : MonoBehaviour
{
    public int startingLife = 10;
    public bool destroyOnKill = false;
    public float delayToKill = 1f;
    private int _currentLife;
    private bool _isDead = false;

    private void Awake()
    {
        _isDead = false;
        _currentLife = startingLife;
    }

    public void TakeDamage(int damage)
    {
        if (_isDead) return;

        _currentLife -= damage;
        if (_currentLife <= 0 )
        {
            Kill();
        }
    }

    private void Kill()
    {
        _isDead = true;
        if (destroyOnKill)
        {
            Destroy(gameObject, delayToKill);
        }
    }
}
