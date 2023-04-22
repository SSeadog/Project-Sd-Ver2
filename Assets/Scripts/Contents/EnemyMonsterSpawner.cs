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
        while (Managers.Game.spawnedEnemyMonsterCount < Managers.Game.SpawnInfo.Count)
        {
            for (int i = Managers.Game.spawnedEnemyMonsterCount; i < Managers.Game.SpawnInfo.Count; i++)
            {
                if (Managers.Game.SpawnInfo[i].spawnTime < Managers.Game.playTime && !Managers.Game.SpawnInfo[i].isSpawned)
                {
                    GameObject instance = null;

                    if (Managers.Game.SpawnInfo[i].type == Define.ObjectType.EnemyMeleeMonster)
                        instance = Managers.Game.Spawn(Define.ObjectType.EnemyMeleeMonster, "Prefabs/Monsters/EnemyMeleeMonster");
                    else if (Managers.Game.SpawnInfo[i].type == Define.ObjectType.EnemyRangedMonster)
                        instance = Managers.Game.Spawn(Define.ObjectType.EnemyRangedMonster, "Prefabs/Monsters/EnemyRangedMonster");

                    instance.transform.rotation = Quaternion.Euler(0, 180, 0);
                    Managers.Game.SpawnInfo[i].isSpawned = true;
                }
                else
                {
                    continue;
                }
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
}
