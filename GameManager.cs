using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static Tank Player1;
    public static Tank Player2;

    public static int Player1Lifes;
    public static int Player2Lifes;

    public static int[] Player1Scores = new int[5] { 0, 0, 0, 0, 0 };
    public static int[] Player2Scores = new int[5] { 0, 0, 0, 0, 0 };

    public static float FieldSizeX = 13f;
    public static float FieldSizeY = 13f;

    public static GameModes GameMode;

    public static bool IsTimeFreezed = false;

    public Transform[] EnemySpawns;
    public Transform[] PlayerSpawns;

    public Level[] Levels;
    public static int currentLevel;
    private int nextEnemy;
    private Coroutine enemyCheckRoutine;

    public static List<Tank> Enemies = new List<Tank>();
    public static int RemainingEnemies;

    [SerializeField]
    private Tank tankPrefab;
    [SerializeField]
    private Buff _buffPrefab;
    private static Buff buffPrefab;
    [SerializeField]
    private GameObject _baseDefensePrefab;
    private static GameObject baseDefensePrefab;

    [SerializeField]
    private static GameManager sceneGM;
    private static Coroutine timeFreezingRoutine;
    private static Coroutine baseStrengthingRoutine;
    private static GameObject baseDefense;

    [SerializeField]
    private GameObject _gameOver;
    private static GameObject gameOver;


    public static void FreezeTime()
    {
        if (timeFreezingRoutine != null) 
        {
            sceneGM.StopCoroutine(timeFreezingRoutine);
        }
        timeFreezingRoutine = sceneGM.StartCoroutine(TimeFreezing());
    }

    public static void StrengthenBase()
    {
        if (baseStrengthingRoutine != null)
        {
            sceneGM.StopCoroutine(baseStrengthingRoutine);
        }

        baseStrengthingRoutine = sceneGM.StartCoroutine(BaseStrengthingRoutine());
    }

    public static void BlowUpEnemies(Tank tank)
    {
        List<Tank> tanks = new List<Tank>();
        foreach (Tank enemy in Enemies)
        {
            tanks.Add(enemy);
        }
        foreach (Tank enemy in tanks)
        {
            enemy.Blow();
        }
    }

    public static void OnTankDestroy(Tank tank)
    {
        if (tank == Player1)
        {
            Player1 = sceneGM.SpawnPlayer(TankSpritesIndex.Player1);
            if (--Player1Lifes < 0)
            {
                GameOver();
            }
        }
        else if (tank == Player2)
        {
            Player2 = sceneGM.SpawnPlayer(TankSpritesIndex.Player2);
            if (--Player2Lifes < 0)
            {
                GameOver();
            }
        }
        else
        {
            if (tank.IsBuffed)
            {
                SpawnBuff();
            }
            Enemies.Remove(tank);
        }
    }

    public static void GameOver()
    {
        gameOver.SetActive(true);
        sceneGM.StartCoroutine(LoadingNewScene());
    }

    public static void AddScore(int player, int type)
    {
        if (player == 1)
        {
            Player1Scores[type]++;
        } 
        else if (player == 2)
        {
            Player2Scores[type]++;
        }
    }

    private static void SpawnBuff()
    {
        float x = Mathf.Round(Random.Range(0, FieldSizeX - 1)*2)/2f;
        float y = Mathf.Round(Random.Range(0, FieldSizeY - 1)*2)/2f;

        int typeInt = Random.Range(0, 6);

        Buff buff = Instantiate(buffPrefab, new Vector2(x, y), Quaternion.identity);
        buff.type = (BuffTypes)typeInt;
    }

    private void SetLevel(int level)
    {
        level %= Levels.Length;

        if (enemyCheckRoutine != null)
        {
            StopCoroutine(enemyCheckRoutine);
        }

        Levels[currentLevel].gameObject.SetActive(false);
        Levels[level].gameObject.SetActive(true);
        currentLevel = level;
        foreach (Tank enemy in Enemies)
        {
            Destroy(enemy.gameObject);
        }
        Enemies.Clear();
        nextEnemy = 0;
        RemainingEnemies = 20;
        enemyCheckRoutine = StartCoroutine(EnemySpawningCheck());
        if (Player1 != null)
        {
            Destroy(Player1.gameObject);
        }
        if (Player2 != null)
        {
            Destroy(Player2.gameObject);
        }
        SpawnPlayers(GameMode);
    }

    private void Start()
    {
        buffPrefab = _buffPrefab;
        gameOver = _gameOver;
        sceneGM = this;
        Player1Lifes = 2;
        if (GameMode == GameModes.TwoPlayers)
        {
            Player2Lifes = 2;
        } 
        else
        {
            Player2Lifes = 0;
        }
        Player1Scores = new int[5] { 0, 0, 0, 0, 0 };
        Player2Scores = new int[5] { 0, 0, 0, 0, 0 };
        SetLevel(0);
        baseDefensePrefab = _baseDefensePrefab;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKey(Keys.MoveRight1))
        {
            Player1.Move(Vector2.right);
        }
        else if (Input.GetKey(Keys.MoveLeft1))
        {
            Player1.Move(Vector2.left);
        }
        else if (Input.GetKey(Keys.MoveDown1))
        {
            Player1.Move(Vector2.down);
        }
        else if (Input.GetKey(Keys.MoveUp1))
        {
            Player1.Move(Vector2.up);
        }
        if (Input.GetKeyDown(Keys.Fire1))
        {
            Player1.Fire();
        }

        if (Player2 != null)
        {
            if (Input.GetKey(Keys.MoveRight2))
            {
                Player2.Move(Vector2.right);
            }
            else if (Input.GetKey(Keys.MoveLeft2))
            {
                Player2.Move(Vector2.left);
            }
            else if (Input.GetKey(Keys.MoveDown2))
            {
                Player2.Move(Vector2.down);
            }
            else if (Input.GetKey(Keys.MoveUp2))
            {
                Player2.Move(Vector2.up);
            }
            if (Input.GetKeyDown(Keys.Fire2))
            {
                Player2.Fire();
            }
        }
    }

    private void SpawnPlayers(GameModes gameMode)
    {
        Player1 = SpawnPlayer(TankSpritesIndex.Player1);

        if (gameMode == GameModes.TwoPlayers)
        {
            Player2 = SpawnPlayer(TankSpritesIndex.Player2);
        }
    }

    private Tank SpawnPlayer(TankSpritesIndex playerNumber)
    {
        int playerSpawn;

        if (playerNumber == TankSpritesIndex.Player1)
        {
            playerSpawn = 0;
        }
        else
        {
            playerSpawn = 1;
        }

        Tank player = Instantiate(tankPrefab, PlayerSpawns[playerSpawn].position, Quaternion.identity);
        player.SetType(TankType.Tier1, playerNumber);
        player.ShieldUp(Constants.SpawnShieldTime, Constants.SpawnAnimationTime);
        return player;
    }

    private void SpawnEnemy(int enemy)
    {
        Tank newEnemy = Instantiate(tankPrefab, EnemySpawns[enemy % EnemySpawns.Length].position, Quaternion.identity);
        Enemies.Add(newEnemy);
        RemainingEnemies--;
        if (enemy == 3 || enemy == 9 || enemy == 17)
        {
            newEnemy.SetType(TankType.EnemyTiersArray[(int)Levels[currentLevel].Sequence[enemy]], TankSpritesIndex.BuffedEnemy);
            newEnemy.IsBuffed = true;
        }
        else
        {
            newEnemy.SetType(TankType.EnemyTiersArray[(int)Levels[currentLevel].Sequence[enemy]], TankSpritesIndex.Enemy);
        }
        newEnemy.StartAutopilot();
    }

    private IEnumerator EnemySpawningCheck()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(Constants.EnemySpawnDelay);

            if (nextEnemy < 20 && Enemies.Count < 4)
            {
                SpawnEnemy(nextEnemy++);
            }
            else if (nextEnemy == 20 && Enemies.Count == 0)
            {
                yield return new WaitForSeconds(Constants.EnemySpawnDelay);
                SetLevel(currentLevel + 1);
            }
        }
    }

    private static IEnumerator TimeFreezing()
    {
        IsTimeFreezed = true;
        yield return new WaitForSeconds(Constants.TimeFreezingDuration);
        IsTimeFreezed = false;
    }

    private static IEnumerator BaseStrengthingRoutine()
    {
        baseDefense = Instantiate(baseDefensePrefab);
        yield return new WaitForSeconds(Constants.BaseStrengthingDuration);
        Destroy(baseDefense);
    }

    private static IEnumerator LoadingNewScene()
    {
        yield return new WaitForSeconds(Constants.TimeUntilLoad);
        SceneManager.LoadScene("Scores");
    }
}

public enum GameModes
{
    OnePlayer,
    TwoPlayers,
}
