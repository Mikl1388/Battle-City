using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPIndex : MonoBehaviour
{
    [SerializeField]
    [Range(1, 2)]
    private int player;

    private Text text;

    void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        if (player == 1)
        {
            text.text = GameManager.Player1Lifes.ToString();
        }
        else if (player == 2)
        {
            text.text = GameManager.Player2Lifes.ToString();
        }
    }
}
