using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public BuffTypes type;
    [SerializeField]
    private Sprite[] sprites;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[(int)type];
        StartCoroutine(Blinking());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Tank>(out Tank tank))
        {
            if (tank == GameManager.Player1 || tank == GameManager.Player2)
            {
                tank.Buff(type);
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator Blinking()
    {
        for (; ; )
        {
            spriteRenderer.color = new Color(255, 255, 255, 0); //transparent
            yield return new WaitForSeconds(0.125f);
            spriteRenderer.color = new Color(255, 255, 255, 255); //oiginal
            yield return new WaitForSeconds(0.125f);
        }
    }
}

public enum BuffTypes
{
    Helmet,
    StopWatch,
    Shovel,
    Star, 
    Grenade,
    Tank
}
