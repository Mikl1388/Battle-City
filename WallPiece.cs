using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPiece : MonoBehaviour
{

    public void TakeDamage(Projectile projectile)
    {
        Vector3 direction = projectile.transform.rotation * Vector2.up; //Получение направление полёта снаряда
        Vector3 perp = new Vector3(direction.y, -direction.x, 0); //Получение перпендикуляра направлению полёта снаряда

        //Этот блок получает список частей стены, соприкасающихся сбоку с теми, в которые попал снаряд, так как снаряд уничтожает части стены в области взрыва
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        hits.AddRange(Physics2D.RaycastAll(transform.position, perp, 2/16f + 0.1f)); //Выпуск луча из центра части стены в сторону перпендикуляра на длину чуть больше половины ширины этой части (т.е. немного выходящего за её границы)
        hits.AddRange(Physics2D.RaycastAll(transform.position, -perp, 2/16f + 0.1f)); // 2/16 is half of wall piece size

        Debug.DrawRay(transform.position, perp * (2 / 16f + 0.1f),Color.green);
        Debug.DrawRay(transform.position, -perp * (2 / 16f + 0.1f), Color.green);

        if (projectile.CanDestroySteel) //Если снаряд может уничтожать сталь, то он был выпущен танком с 3 звёздами и должен уничтожить ещё один слой стены
        {
            //Уничтожение частей стен, которые находятся за теми, в которые было произведено попадание
            Debug.DrawRay(transform.position, direction * (2 / 16f + 0.1f), Color.red);
            foreach (RaycastHit2D hit in Physics2D.RaycastAll(transform.position, direction, 2 / 16f + 0.1f))
            {
                if (hit.fraction != 0)
                {
                    if (hit.collider.gameObject.TryGetComponent<WallPiece>(out WallPiece otherWall))
                    {
                        projectile.CanDestroySteel = false;
                        otherWall.TakeDamage(projectile);
                    }
                }
            }
        }

        //Уничтожение частей стен, которые находятся сбоку от тех, в которые было произведено попадание
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.fraction != 0)
            {
                if (hit.collider.gameObject.TryGetComponent<WallPiece>(out WallPiece otherWall))
                {
                    otherWall.BreakDown();
                }
            }
        }
        BreakDown();
    }

    public void BreakDown()
    {
        Destroy(gameObject, Time.deltaTime);
    }
}
