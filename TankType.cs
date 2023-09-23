using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class TankType
{
    public float TankSpeed = DefaultTankSpeed;
    public int MaxHealth = DefaultMaxHealth;
    public float ProjectileSpeed = DefaultProjectileSpeed;
    public int MaxProjectileCount = DefaultMaxProjectileCount;
    public bool CanDestroySteel = DefaultCanDestroySteel;
    public int SpriteIndexOffset;

    public static TankType Tier1 = new TankType(DefaultTankSpeed, DefaultMaxHealth, DefaultProjectileSpeed, DefaultMaxProjectileCount, DefaultCanDestroySteel, 0);
    public static TankType Tier2 = new TankType(DefaultTankSpeed, DefaultMaxHealth, UpgradedProjectileSpeed, DefaultMaxProjectileCount, DefaultCanDestroySteel, 1);
    public static TankType Tier3 = new TankType(DefaultTankSpeed, DefaultMaxHealth, UpgradedProjectileSpeed, UpgradedMaxProjectileCount, DefaultCanDestroySteel, 2);
    public static TankType Tier4 = new TankType(DefaultTankSpeed, DefaultMaxHealth, UpgradedProjectileSpeed, UpgradedMaxProjectileCount, UpgradedCanDestroySteel, 3);

    public static TankType[] PlayersTiersArray = new TankType[] { Tier1, Tier2, Tier3, Tier4 };


    public static TankType Enemy1 = new TankType(DefaultTankSpeed, DefaultMaxHealth, DefaultProjectileSpeed, DefaultMaxProjectileCount, DefaultCanDestroySteel, 0);
    public static TankType Enemy2 = new TankType(UpgradedTankSpeed, DefaultMaxHealth, DefaultProjectileSpeed, DefaultMaxProjectileCount, DefaultCanDestroySteel, 1);
    public static TankType Enemy3 = new TankType(DefaultTankSpeed, DefaultMaxHealth, UpgradedProjectileSpeed, DefaultMaxProjectileCount, DefaultCanDestroySteel, 2);
    public static TankType Enemy4 = new TankType(DefaultTankSpeed, UpgradedMaxHealth, DefaultProjectileSpeed, DefaultMaxProjectileCount, DefaultCanDestroySteel, 3);

    public static TankType[] EnemyTiersArray = new TankType[] { Enemy1, Enemy2, Enemy3, Enemy4 };

    public TankType(float tankSpeed, int maxHealth, float projectileSpeed, int maxProjectileCount, bool canDestroySteel, int spriteIndexOffset)
    {
        TankSpeed = tankSpeed;
        MaxHealth = maxHealth;
        ProjectileSpeed = projectileSpeed;
        MaxProjectileCount = maxProjectileCount;
        CanDestroySteel = canDestroySteel;
        SpriteIndexOffset = spriteIndexOffset;
    }
}


public enum TankSpritesIndex
{
    Player1 = 0,
    Player2 = 4,
    Enemy = 8,
    BuffedEnemy = 12
}