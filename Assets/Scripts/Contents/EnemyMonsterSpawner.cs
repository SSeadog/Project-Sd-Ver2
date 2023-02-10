using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyMonsterSpawner : MonoBehaviour
{
    public class spawnItem
    {
        public Define.ObjectType type;
        public float spawnTime;
        public bool isSpawned = false;
    }

    // type, time, isSpawned
    List<spawnItem> spawnInfo;
    int curSpawnCount = 0;

    void Start()
    {
        //StreamReader reader = new StreamReader(Path.Combine(Application.streamingAssetsPath, "GameSettings/Stages/Spawn_" + Managers.Game.stageNum + ".json"));
        //string json = reader.ReadToEnd();
        //spawnInfo = JsonConvert.DeserializeObject<List<spawnItem>>(json);
        spawnInfo = Util.LoadJsonList<List<spawnItem>>("GameSettings/Stages/Spawn_1");
    }

    void Update()
    {
        //if (Managers.Game._isGameWin || Managers.Game._isGameLose)
        //{
        //    return;
        //}

        for (int i = curSpawnCount; i < spawnInfo.Count; i++)
        {
            if (spawnInfo[i].spawnTime < Managers.Game.playTime && !spawnInfo[i].isSpawned)
            {
                if (spawnInfo[i].type == Define.ObjectType.EnemyMeleeMonster)
                {
                    GameObject instance = Managers.Game.Spawn(Define.ObjectType.EnemyMeleeMonster, "Prefabs/Monsters/EnemyMeleeMonster");
                    instance.transform.position = Managers.Game.enemyTower.GetComponent<EnemyTowerController>().GetSpawnRoot().position;
                    curSpawnCount++;
                }

                spawnInfo[i].isSpawned = true;
            }
            else
            {
                continue;
            }
        }
    }

    public void ReSetSpawnInfo()
    {
        curSpawnCount = 0;

        for (int i = 0; i < spawnInfo.Count; i++)
        {
            spawnInfo[i].isSpawned = false;
        }
    }
}
