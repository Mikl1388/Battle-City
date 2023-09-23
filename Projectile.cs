using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Speed;
    public bool CanDestroySteel;
    public Tank Parent;

    private Animator animator;
    [SerializeField]
    private AnimationClip destructionClip;

    private Collider2D coll;

    private void Start()
    {
        animator = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        transform.Translate(Speed * Time.fixedDeltaTime * Vector2.up);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject != Parent.gameObject)
        {
            if (collision.gameObject.TryGetComponent<Projectile>(out _))
            {
                Destroy(gameObject);
                return;
            }
            if (collision.gameObject.TryGetComponent<WallPiece>(out WallPiece wall))
            {
                wall.TakeDamage((Projectile)MemberwiseClone());
            }
            if (collision.gameObject.TryGetComponent<Tank>(out Tank tank))
            {
                tank.TakeDamage(Parent);
            }
            if (collision.gameObject.TryGetComponent<Steel>(out Steel steel))
            {
                steel.TakeDamage(this);
            }
            if (collision.gameObject.TryGetComponent<Flag>(out Flag flag))
            {
                flag.TakeDamage();
            }

            Blow();
        }
    }

    private void OnDestroy()
    {
        Parent.DecreaseProjectilesCount();
    }

    private void Blow()
    {
        coll.enabled = false;
        Speed = 0;
        animator.SetBool("Exploded", true);
        Destroy(gameObject, destructionClip.length);
    }
}
