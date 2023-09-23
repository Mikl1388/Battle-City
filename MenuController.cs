using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] cursors;

    private int currentState;

    void Update()
    {
        if (Input.GetKeyDown(Keys.MoveDown1) || Input.GetKeyDown(Keys.MoveDown2))
        {
            currentState = (currentState + 1) % 4;
        }
        else if (Input.GetKeyDown(Keys.MoveUp1) || Input.GetKeyDown(Keys.MoveUp2))
        {
            if (--currentState == -1)
            {
                currentState = 3;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (currentState)
            {
                case 0:
                    GameManager.GameMode = GameModes.OnePlayer;
                    GameManager.Enemies.Clear();
                    SceneManager.LoadScene(1);
                    break;
                case 1:
                    GameManager.GameMode = GameModes.TwoPlayers;
                    GameManager.Enemies.Clear();
                    SceneManager.LoadScene(1);
                    break;
                case 2:
                    SceneManager.LoadScene(3);
                    break;
                case 3:
                    SceneManager.LoadScene(4);
                    break;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        for (int i = 0; i < cursors.Length; i++)
        {
            if (i != currentState)
            {
                cursors[i].SetActive(false);
            }
            else
            {
                cursors[i].SetActive(true);
            }
        }
    }
}
