using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ExitButton : MonoBehaviour
{

    Text text;

    [SerializeField]
    InputField player1;
    [SerializeField]
    InputField player2;
    string datapath;

    void Start()
    {
        text = GetComponent<Text>();
        datapath = Application.persistentDataPath + "/leaderboard.lbd";
    }


    private void OnMouseDown()
    {
        //datapath = Application.persistentDataPath + "/leaderboard.lbd";
        //������ ���� ���������� � ������ ������, ��� ������� �����
        Dictionary<string, int> leaderboard = new Dictionary<string, int>();
        if (File.Exists(datapath)) //���� ���� ��� ����������, �� ���� ����������� ��������
        {
            leaderboard = Constants.ReadLeaderboard(datapath);
        }

        //� ������� ����������� �������� �� ��������� ����
        leaderboard.Add(player1.text, Scores.Player1Score);
        if (GameManager.GameMode == GameModes.TwoPlayers)
        {
            leaderboard.Add(player2.text, Scores.Player2Score);
        }

        //������� ����������� �� ��������� � ������� �������� � �������������� ����������� System.Linq
        var sorted = from item in leaderboard orderby item.Value descending select item;
        List<string> newdata = new List<string>();
        int i = 0;

        foreach (var item in sorted)
        {
            if (++i > 16) //� ������� ������ 16 ������ �����������, ��������� ������������
            {
                break;
            }
            string temp = $"{item.Key} {item.Value}";
            newdata.Add(temp);
        }

        File.WriteAllLines(datapath, newdata); //� ���� ������������ ����� ����������
        SceneManager.LoadScene(0);
    }

    private void OnMouseEnter()
    {
        text.color = new Color(150f / 255, 34f / 255, 39f / 255);
    }

    private void OnMouseExit()
    {
        text.color = new Color(181f / 255, 40f / 255, 47f / 255);
    }
}