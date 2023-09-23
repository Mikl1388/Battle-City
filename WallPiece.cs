using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPiece : MonoBehaviour
{

    public void TakeDamage(Projectile projectile)
    {
        Vector3 direction = projectile.transform.rotation * Vector2.up; //��������� ����������� ����� �������
        Vector3 perp = new Vector3(direction.y, -direction.x, 0); //��������� �������������� ����������� ����� �������

        //���� ���� �������� ������ ������ �����, ��������������� ����� � ����, � ������� ����� ������, ��� ��� ������ ���������� ����� ����� � ������� ������
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        hits.AddRange(Physics2D.RaycastAll(transform.position, perp, 2/16f + 0.1f)); //������ ���� �� ������ ����� ����� � ������� �������������� �� ����� ���� ������ �������� ������ ���� ����� (�.�. ������� ���������� �� � �������)
        hits.AddRange(Physics2D.RaycastAll(transform.position, -perp, 2/16f + 0.1f)); // 2/16 is half of wall piece size

        Debug.DrawRay(transform.position, perp * (2 / 16f + 0.1f),Color.green);
        Debug.DrawRay(transform.position, -perp * (2 / 16f + 0.1f), Color.green);

        if (projectile.CanDestroySteel) //���� ������ ����� ���������� �����, �� �� ��� ������� ������ � 3 ������� � ������ ���������� ��� ���� ���� �����
        {
            //����������� ������ ����, ������� ��������� �� ����, � ������� ���� ����������� ���������
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

        //����������� ������ ����, ������� ��������� ����� �� ���, � ������� ���� ����������� ���������
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
