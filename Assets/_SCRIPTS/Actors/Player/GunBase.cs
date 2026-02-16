using System;
using Unity.VisualScripting;
using UnityEngine;

public class GunBase : MonoBehaviour
{
    public ProjectileBase projectile;
    public Transform positionToShoot;
    [NonSerialized] public bool facingLeft = false;

    public void Shoot()
    {
        var obj = Instantiate(projectile);
        obj.transform.position = positionToShoot.position;
        if (facingLeft)
        {
            obj.direction = new Vector3(-obj.direction.x, obj.direction.y, obj.direction.z);
        }
        
    }
}
