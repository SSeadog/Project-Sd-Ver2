using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyMonsterSpawner : MonoBehaviour
{
    void Start()
    {
        //StreamReader reader = new StreamReader(Path.Combine(Application.streamingAssetsPath, "GameSettings/Stages/Spawn_" + Managers.Game.stageNum + ".json"));
        //string json = reader.ReadToEnd();
        //spawnInfo = JsonConvert.DeserializeObject<List<spawnItem>>(json);
    }

    void Update()
    {
        //if (Managers.Game._isGameWin || Managers.Game._isGameLose)
        //{
        //    return;
        //}

        for (int i = Managers.Game.spawnedEnemyMonsterCount; i < Managers.Game.spawnInfo.Count; i++)
        {
            if (Managers.Game.spawnInfo[i].spawnTime < Managers.Game.playTime && !Managers.Game.spawnInfo[i].isSpawned)
            {
                if (Managers.Game.spawnInfo[i].type == Define.ObjectType.EnemyMeleeMonster)
                {
                    GameObject instance = Managers.Game.Spawn(Define.ObjectType.EnemyMeleeMonster, "Prefabs/Monsters/EnemyMeleeMonster");
                    instance.transform.position = Managers.Game.enemyTower.GetComponent<EnemyTowerController>().GetSpawnRoot().position;
                    Managers.Game.spawnedEnemyMonsterCount++;
                }

                Managers.Game.spawnInfo[i].isSpawned = true;
            }
            else
            {
                continue;
            }
        }
    }
}
