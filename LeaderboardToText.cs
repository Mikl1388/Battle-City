using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardToText : MonoBehaviour
{
    Dictionary<string, int> leaderboard;
    string path;

    [SerializeField]
    Text text1;
    [SerializeField]
    Text text2;

    void Start()
    {
        path = Application.persistentDataPath + "/leaderboard.lbd";
        leaderboard = Constants.ReadLeaderboard(path);

        int i = 0;
        string t1 = "";
        string t2 = "";
        
        foreach (var item in leaderboard)
        {
            if (i++ < 8)
            {
                t1 += $"{i}.  {item.Key}  -  {item.Value}\n";
            }
            else
            {
                t2 += $"{i}.  {item.Key}  -  {item.Value}\n";
            }
        }
        text1.text = t1;
        text2.text = t2;
    }
}
