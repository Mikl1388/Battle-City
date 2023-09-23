using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Constants
{
    public static float StunTime = 4.5f;
    public static float SpawnShieldTime = 3.5f;
    public static float BuffShieldTime = 10f;
    public static float SpawnAnimationTime = 1.267f;
    public static float EnemySpawnDelay = 2f;
    public static float TimeFreezingDuration = 10f;
    public static float BaseStrengthingDuration = 20f;

    public static float DefaultTankSpeed = 3f;
    public static int DefaultMaxHealth = 1;
    public static float DefaultProjectileSpeed = 8.5f;
    public static int DefaultMaxProjectileCount = 1;
    public static bool DefaultCanDestroySteel = false;

    public static float UpgradedTankSpeed = 6f;
    public static int UpgradedMaxHealth = 4;
    public static float UpgradedProjectileSpeed = 17f;
    public static int UpgradedMaxProjectileCount = 2;
    public static bool UpgradedCanDestroySteel = true;

    public static float MinAutopilotDelay = 0f;
    public static float MaxAutopilotDelay = 1.5f;
    public static Vector2[] AutopilotDirections = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right};

    public static float TimeUntilLoad = 3f;

    public static int[] ScoreValues = new int[] { 100, 200, 300, 400, 500 };

    public static Dictionary<string, int> ReadLeaderboard(string path)
    {
        string[] data = File.ReadAllLines(path); //Считывание всего файла построчно
        Dictionary<string, int> leaderboard = new Dictionary<string, int>();

        //Запись из файла в словарь парами <Имя, Количество очков>
        foreach (string line in data)
        {
            string key = line.Split(" ")[0];
            int value = int.Parse(line.Split(" ")[1]);
            leaderboard.Add(key, value);
        }
        return leaderboard;
    }
}
