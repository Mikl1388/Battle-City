using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steel : MonoBehaviour
{
    public void TakeDamage(Projectile projectile)
    {
        if (projectile.CanDestroySteel)
        {
            Destroy(gameObject);
        }
    }
}
