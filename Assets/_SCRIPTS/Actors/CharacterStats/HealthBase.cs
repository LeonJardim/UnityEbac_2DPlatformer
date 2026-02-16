using UnityEngine;

public class HealthBase : MonoBehaviour
{
    public int startingLife = 10;
    public bool destroyOnKill = true;
    public float delayToKill = 1f;

    private FlashColor _flashColor;
    private int _currentLife;
    private bool _isDead = false;


    private void Awake()
    {
        _isDead = false;
        _currentLife = startingLife;
        if (_flashColor == null)
        {
            _flashColor = GetComponent<FlashColor>();
        }
    }

    public void TakeDamage(int damage)
    {
        if (_isDead) return;

        _currentLife -= damage;
        if (_currentLife <= 0 )
        {
            Kill();
        }

        if (_flashColor != null)
        {
            _flashColor.Flash();
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
