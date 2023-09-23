using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    public Vector2 StartPosition;
    public float ProjectileSpeed;
    public int MaxProjectileCount;
    public bool CanDestroySteel;
    public TankType ThisTankType;
    public TankSpritesIndex ThisTankSpriteIndex;

    public int Health;

    public bool IsBuffed;

    private float MoveSpeed;

    [SerializeField]
    private Projectile projectilePrefab;
    [SerializeField]
    private Vector3 projectileStart;

    private int projectilesNumber;

    private float spawnTimer = Constants.SpawnAnimationTime; // spawn animation duration

    private float stunTimer = 0;
    private bool isStunned;
    private SpriteRenderer spriteRenderer;
    private Coroutine blinkingRoutine;

    private GameObject shield;
    private bool shielded;

    private Collider2D coll;

    private Animator animator;
    private bool isMoving;

    [SerializeField]
    private Sprite[] animationSprites;
    private int currentAnimationState;

    private bool isAutopiloting;
    private Vector2 autopilotDirection = Vector2.up;

    [SerializeField]
    private AnimationClip destructionClip;

    public void Spawn(Vector2 startPosition, TankType type)
    {
        StartPosition = startPosition;
        SetType(type);
        ShieldUp(Constants.SpawnShieldTime);
    }

    public void SetType(TankType type)
    {
        MoveSpeed = type.TankSpeed;
        Health = type.MaxHealth;
        ProjectileSpeed = type.ProjectileSpeed;
        MaxProjectileCount = type.MaxProjectileCount;
        CanDestroySteel = type.CanDestroySteel;
        animationSprites = SpritesHolder.AnimationSpritesList[(int)ThisTankSpriteIndex + type.SpriteIndexOffset];
        ThisTankType = type;
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = animationSprites[0];
        }
    }

    public void SetType(TankType type, TankSpritesIndex spriteIndex)
    {
        ThisTankSpriteIndex = spriteIndex;
        SetType(type);
    }

    public void StartAutopilot()
    {
        isAutopiloting = true;
        StartCoroutine(Autopiloting());
    }

    public void Move(Vector2 direction)
    {
        if (isStunned) { return; } //Танк не может ехать, если оглушён, например, из-за выстрела союзника

        isMoving = true; //Изменение состояния анимации

        if (direction == Vector2.up || direction == Vector2.right) //Если танк едет вниз или влево, его изображение отзеркаливатся для создания иллюзии освещения
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(Vector3.back, direction);
        }

        //Округление значений координат в направлении, перпендикулярном движению до 0.5.
        if (direction.x != 0)
        {
            transform.position = new Vector2(transform.position.x, Mathf.Round(transform.position.y * 2) / 2); 
        }
        else if (direction.y != 0)
        {
            transform.position = new Vector2(Mathf.Round(transform.position.x * 2) / 2, transform.position.y);
        }

        //Следующий блок отвечает за проверку столкновений путём выпуска трёх лучей из переда танка.
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        hits.AddRange(Physics2D.RaycastAll(transform.position, direction, 0.5001f));
        hits.AddRange(Physics2D.RaycastAll(transform.position + transform.rotation * Vector3.left * 0.25f, direction, 0.5001f));
        hits.AddRange(Physics2D.RaycastAll(transform.position + transform.rotation * Vector3.right * 0.25f, direction, 0.5001f));

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.fraction != 0) { return; } //Если луч встречает на пути объект, перемещение отменяется.
        }

        transform.Translate(Time.deltaTime * MoveSpeed * Vector2.up); //Смещение танка вперёд по направлению "взгляда"
    }

    public void Fire()
    {
        if (isStunned) { return; } //Танк не может стрелять, если оглушён
        if (projectilesNumber >= MaxProjectileCount) { return; } //Одновременно танк может выпустить только 1 снаряд (игрок может 2 при подборе 2 звёзд)

        Projectile projectile = Instantiate(projectilePrefab, transform.position + (transform.rotation * ((1 + Time.deltaTime) * projectileStart)), transform.rotation); //Создание игрового объекта снаряда
        projectile.Parent = this; //Сообщение снаряду информации о танке, который его выпустил
        projectile.Speed = ProjectileSpeed; //Задание скорости снаряда
        projectile.CanDestroySteel = CanDestroySteel; //Сообщение снаряду информации о том, может ли он пробивать сталь
        projectilesNumber++; //Подсчёт выпущенных снарядов
    }

    public void DecreaseProjectilesCount()
    {
        projectilesNumber--;
    }

    public void TakeDamage(Tank shooter)
    {
        if (isStunned) { return; }
        if (shielded) { return; }
        if (shooter == GameManager.Player1 || shooter == GameManager.Player2)
        {
            if (this == GameManager.Player1 || this == GameManager.Player2)
            {
                Stun();
            }
            else
            {
                if (--Health < 1)
                {
                    int player = shooter == GameManager.Player1 ? 1 : 2;
                    GameManager.AddScore(player, ThisTankType.SpriteIndexOffset);
                    Blow();
                }
            }
        }
        else if (this == GameManager.Player1 || this == GameManager.Player2)
        {
            Blow();
        }

    }

    public void Respawn()
    {
        Stun();
        stunTimer = Constants.StunTime / 2f;
        transform.position = StartPosition;
    }

    public void Stun()
    {
        stunTimer = Constants.StunTime;
        StopCoroutine(blinkingRoutine);
        blinkingRoutine = StartCoroutine(Blinking());
    }

    public void ShieldUp(float time)
    {
        shielded = true;
        shield.SetActive(true);
        StartCoroutine(Shielding(time));
    }

    public void ShieldUp(float time, float delay)
    {
        StartCoroutine(DelayedShielding(time, delay));
    }

    public void Buff(BuffTypes buff)
    {
        int player = this == GameManager.Player1 ? 1 : 2;
        GameManager.AddScore(player, 4);
        switch (buff)
        {
            case BuffTypes.Helmet:
                ShieldUp(Constants.BuffShieldTime);
                break;
            case BuffTypes.StopWatch:
                GameManager.FreezeTime();
                break;
            case BuffTypes.Shovel:
                GameManager.StrengthenBase();
                break;
            case BuffTypes.Star:
                Upgrade();
                break;
            case BuffTypes.Grenade:
                GameManager.BlowUpEnemies(this);
                break;
            case BuffTypes.Tank:
                if (this == GameManager.Player1)
                {
                    GameManager.Player1Lifes++;
                } 
                else if (this == GameManager.Player2)
                {
                    GameManager.Player2Lifes++;
                }
                break;
        }
    }

    public void Upgrade()
    {
        if (ThisTankType == TankType.Tier1)
        {
            SetType(TankType.Tier2);
        }
        else if (ThisTankType == TankType.Tier2)
        {
            SetType(TankType.Tier3);
        }
        else if (ThisTankType == TankType.Tier3)
        {
            SetType(TankType.Tier4);
        }
    }

    private void ShieldDown()
    {
        shielded = false;
        shield.SetActive(false);
    }

    private void Start()
    {
        shield = transform.GetChild(0).gameObject;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        blinkingRoutine = StartCoroutine(Blinking());
        StartCoroutine(MovingAnimation());
        StartCoroutine(AnimatorDisabling(Constants.SpawnAnimationTime));
    }

    private void Update()
    {

        if (stunTimer > 0)
        {
            stunTimer -= Time.deltaTime;
        } 
        else
        {
            stunTimer = 0;
            StopCoroutine(blinkingRoutine);
            spriteRenderer.color = new Color(255, 255, 255, 255);
        }

        if (spawnTimer > 0)
        {
            spawnTimer -= Time.deltaTime;
        }
        else
        {
            spawnTimer = 0;
        }

        if (stunTimer == 0 && spawnTimer == 0)
        {
            isStunned = false;
        }
        else
        {
            isStunned = true;
        }
        
        if (isAutopiloting && !GameManager.IsTimeFreezed)
        {
            Move(autopilotDirection);
        }
    }
    public void Blow()
    {
        StopAllCoroutines();
        GameManager.OnTankDestroy(this);
        isAutopiloting = false;
        coll.enabled = false;
        animator.enabled = true;
        animator.SetBool("Blown", true);
        Destroy(gameObject, destructionClip.length);
    }

    private IEnumerator Blinking()
    {
        for (; ; )
        {
            spriteRenderer.color = new Color(255, 255, 255, 0); //transparent
            yield return new WaitForSeconds(0.25f);
            spriteRenderer.color = new Color(255, 255, 255, 255); //oiginal
            yield return new WaitForSeconds(0.25f);
        }
    }

    private IEnumerator Shielding(float time)
    {
        yield return new WaitForSeconds(time);
        ShieldDown();
    }

    private IEnumerator DelayedShielding(float time, float delay)
    {
        yield return new WaitForSeconds(delay);
        shielded = true;
        shield.SetActive(true);
        spriteRenderer.sprite = animationSprites[0];
        animator.enabled = false;
        coll.enabled = true;
        yield return new WaitForSeconds(time);
        ShieldDown();
    }

    private IEnumerator MovingAnimation()
    {
        for (; ; )
        {
            if (isMoving)
            {

                currentAnimationState = (currentAnimationState + 1) % animationSprites.Length;
                spriteRenderer.sprite = animationSprites[currentAnimationState];
                isMoving = false;
                animator.enabled = false;
                coll.enabled = true;
            }
            yield return new WaitForSeconds(0.075f);
        }
    }

    private IEnumerator Autopiloting()
    {
        for (; ; )
        {
            AutopilotCommands autopilotCommand = (AutopilotCommands)Random.Range(0, 2);
            if (autopilotCommand == AutopilotCommands.Turn && !GameManager.IsTimeFreezed)
            {
                autopilotDirection = Constants.AutopilotDirections[Random.Range(0, Constants.AutopilotDirections.Length)];
            } 
            else if (autopilotCommand == AutopilotCommands.Shoot && !GameManager.IsTimeFreezed)
            {
                Fire();
            }
            float delay = Random.Range(Constants.MinAutopilotDelay, Constants.MaxAutopilotDelay);
            yield return new WaitForSeconds(delay);
        }
    }

    private IEnumerator AnimatorDisabling(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.enabled = false;
        spriteRenderer.sprite = animationSprites[currentAnimationState];
    }
}

enum AutopilotCommands
{
    Turn,
    Shoot
}