using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public Vector3 direction = Vector3.right;
    public float lifeTime = 1.0f;


    private void Awake()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(direction * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var enemy = collision.transform.GetComponent<HealthBase>();
        if (enemy != null)
        {
            enemy.TakeDamage(5);
        }
        Destroy(gameObject);
    }
}
