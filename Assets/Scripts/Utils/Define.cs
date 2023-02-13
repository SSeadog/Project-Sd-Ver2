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

    public class SettingInfo
    {
        public float PosX;
        public float PosY;
        public float PosZ;
        public float RotX;
        public float RotY;
        public float RotZ;
        public string path;
    }

    public class spawnItem
    {
        public ObjectType type;
        public float spawnTime;
        public bool isSpawned = false;
    }
}
