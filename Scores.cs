using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scores : MonoBehaviour
{
    [SerializeField]
    private GameObject player1;
    [SerializeField]
    private GameObject player2;

    [SerializeField]
    private Text[] player1Texts;
    [SerializeField]
    private Text[] player2Texts;

    public static int Player1Score;
    public static int Player2Score;

    void Start()
    {
        Player1Score = 0;
        Player2Score = 0;
        if (GameManager.GameMode == GameModes.OnePlayer)
        {
            player2.SetActive(false);
        }
        else
        {
            player2.SetActive(true);
        }

        for(int i = 0; i < 5; i++)
        {
            int score = GameManager.Player1Scores[i] * Constants.ScoreValues[i];
            player1Texts[i].text = $"x  {GameManager.Player1Scores[i]}  =  {score}";
            Player1Score += score;
        }
        player1Texts[5].text = Player1Score.ToString();

        if (GameManager.GameMode == GameModes.TwoPlayers)
        {
            for (int i = 0; i < 5; i++)
            {
                int score = GameManager.Player2Scores[i] * Constants.ScoreValues[i];
                player2Texts[i].text = $"x  {GameManager.Player2Scores[i]}  =  {score}";
                Player2Score += score;
            }
            player2Texts[5].text = Player2Score.ToString();
        }
    }
}
