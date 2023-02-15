using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyMonsterSpawner : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(CoSpawning());
    }

    IEnumerator CoSpawning()
    {
        while (Managers.Game.spawnedEnemyMonsterCount < Managers.Game.spawnInfo.Count)
        {
            for (int i = Managers.Game.spawnedEnemyMonsterCount; i < Managers.Game.spawnInfo.Count; i++)
            {
                if (Managers.Game.spawnInfo[i].spawnTime < Managers.Game.playTime && !Managers.Game.spawnInfo[i].isSpawned)
                {
                    if (Managers.Game.spawnInfo[i].type == Define.ObjectType.EnemyMeleeMonster)
                        Managers.Game.Spawn(Define.ObjectType.EnemyMeleeMonster, "Prefabs/Monsters/EnemyMeleeMonster");
                    else if (Managers.Game.spawnInfo[i].type == Define.ObjectType.EnemyRangedMonster)
                        Managers.Game.Spawn(Define.ObjectType.EnemyRangedMonster, "Prefabs/Monsters/EnemyRangedMonster");

                    Managers.Game.spawnInfo[i].isSpawned = true;
                }
                else
                {
                    continue;
                }
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    //void Update()
    //{
    //    if (Managers.Game.spawnInfo == null)
    //        return;

    //    for (int i = Managers.Game.spawnedEnemyMonsterCount; i < Managers.Game.spawnInfo.Count; i++)
    //    {
    //        if (Managers.Game.spawnInfo[i].spawnTime < Managers.Game.playTime && !Managers.Game.spawnInfo[i].isSpawned)
    //        {
    //            if (Managers.Game.spawnInfo[i].type == Define.ObjectType.EnemyMeleeMonster)
    //                Managers.Game.Spawn(Define.ObjectType.EnemyMeleeMonster, "Prefabs/Monsters/EnemyMeleeMonster");
    //            else if (Managers.Game.spawnInfo[i].type == Define.ObjectType.EnemyRangedMonster)
    //                Managers.Game.Spawn(Define.ObjectType.EnemyRangedMonster, "Prefabs/Monsters/EnemyRangedMonster");

    //            Managers.Game.spawnInfo[i].isSpawned = true;
    //        }
    //        else
    //        {
    //            continue;
    //        }
    //    }
    //}
}
