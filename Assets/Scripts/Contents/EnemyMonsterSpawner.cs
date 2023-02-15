using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyMonsterSpawner : MonoBehaviour
{
    void Start()
    {
        Invoke("SpawnTest", 1f);
    }

    void SpawnTest()
    {
        //Managers.Game.Spawn(Define.ObjectType.EnemyMeleeMonster, "Prefabs/Monsters/EnemyMeleeMonster");
        //Managers.Game.Spawn(Define.ObjectType.EnemyRangedMonster, "Prefabs/Monsters/EnemyRangedMonster");
        Managers.Game.Spawn(Define.ObjectType.EnemyPowerMonster, "Prefabs/Monsters/EnemyPowerMonster");
    }

    void Update()
    {
        //for (int i = Managers.Game.spawnedEnemyMonsterCount; i < Managers.Game.spawnInfo.Count; i++)
        //{
        //    if (Managers.Game.spawnInfo[i].spawnTime < Managers.Game.playTime && !Managers.Game.spawnInfo[i].isSpawned)
        //    {
        //        if (Managers.Game.spawnInfo[i].type == Define.ObjectType.EnemyMeleeMonster)
        //            Managers.Game.Spawn(Define.ObjectType.EnemyMeleeMonster, "Prefabs/Monsters/EnemyMeleeMonster");
        //        else if (Managers.Game.spawnInfo[i].type == Define.ObjectType.EnemyRangedMonster)
        //            Managers.Game.Spawn(Define.ObjectType.EnemyRangedMonster, "Prefabs/Monsters/EnemyRangedMonster");

        //        Managers.Game.spawnInfo[i].isSpawned = true;
        //    }
        //    else
        //    {
        //        continue;
        //    }
        //}
    }
}
