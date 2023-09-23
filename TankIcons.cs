using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankIcons : MonoBehaviour
{
    Image[] images;

    void Start()
    {
        images = GetComponentsInChildren<Image>();
    }

    void Update()
    {
        for (int i = 0; i < images.Length; i++)
        {
            if (i < GameManager.RemainingEnemies)
            {
                images[i].enabled = true;
            }
            else
            {
                images[i].enabled = false;
            }
        }
    }
}
