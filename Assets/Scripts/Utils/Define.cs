using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum ObjectType
    {
        None,
        FriendlyTower,
        EnemyTower,
        Player,
        FriendlyMeleeMonster,
        EnemyMeleeMonster
    }

    public enum TagName
    {
        Player,
        FriendlyMonster,
        FriendlyProjectile,
        EnemyMonster,
        EnemyProjectile,
    }
}
