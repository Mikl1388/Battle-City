using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritesHolder : MonoBehaviour
{
    public static Sprite[][] AnimationSpritesList = new Sprite[16][];

    [SerializeField]
    private SpriteArrays spriteArrays;

    private void Awake()
    {
        AnimationSpritesList[0] = spriteArrays.Player1Tier1;
        AnimationSpritesList[1] = spriteArrays.Player1Tier2;
        AnimationSpritesList[2] = spriteArrays.Player1Tier3;
        AnimationSpritesList[3] = spriteArrays.Player1Tier4;

        AnimationSpritesList[4] = spriteArrays.Player2Tier1;
        AnimationSpritesList[5] = spriteArrays.Player2Tier2;
        AnimationSpritesList[6] = spriteArrays.Player2Tier3;
        AnimationSpritesList[7] = spriteArrays.Player2Tier4;

        AnimationSpritesList[8] = spriteArrays.EnemyTier1;
        AnimationSpritesList[9] = spriteArrays.EnemyTier2;
        AnimationSpritesList[10] = spriteArrays.EnemyTier3;
        AnimationSpritesList[11] = spriteArrays.EnemyTier4;

        AnimationSpritesList[12] = spriteArrays.BuffedEnemyTier1;
        AnimationSpritesList[13] = spriteArrays.BuffedEnemyTier2;
        AnimationSpritesList[14] = spriteArrays.BuffedEnemyTier3;
        AnimationSpritesList[15] = spriteArrays.BuffedEnemyTier4;

    }
}
