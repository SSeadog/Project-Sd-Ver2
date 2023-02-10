using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : MonoBehaviour
{
    void Start()
    {
        GameObject playerTowerOriginal = Resources.Load<GameObject>("Prefabs/PlayerTower");
        GameObject playerTower = Instantiate(playerTowerOriginal);
        playerTower.GetComponent<PlayerTowerController>().Init();

        GameObject enemyTowerOriginal = Resources.Load<GameObject>("Prefabs/EnemyTower");
        GameObject enemyTower = Instantiate(enemyTowerOriginal);
        enemyTower.GetComponent<EnemyTowerController>().Init();

        for (int i = 0; i < 10; i++)
        {
            GameObject enemyMonster = Managers.Game.Spawn(Define.ObjectType.EnemyMeleeMonster, "Prefabs/Monsters/EnemyMeleeMonster");
            enemyMonster.transform.position = Managers.Game.enemyTower.GetComponent<EnemyTowerController>().GetSpawnRoot().position;
        }
        
        for (int i = 0; i < 8; i++)
        {
            GameObject playerMonster = Managers.Game.Spawn(Define.ObjectType.PlayerMeleeMonster, "Prefabs/Monsters/PlayerMeleeMonster");
            playerMonster.transform.position = Managers.Game.playerTower.GetComponent<PlayerTowerController>().GetSpawnRoot().position;
        }
    }

    void Update()
    {
        
    }
}
