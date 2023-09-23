using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public EnemyTiers[] Sequence;
}

public enum EnemyTiers
{
    Tier1,
    Tier2,
    Tier3,
    Tier4
}
