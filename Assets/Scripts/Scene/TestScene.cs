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

        GameObject enemyMonsterOriginal = Resources.Load<GameObject>("Prefabs/Monsters/EnemyMeleeMonster");
        GameObject enemyMonster = Instantiate(enemyMonsterOriginal);
        enemyMonster.transform.position = new Vector3(0, 0, 30f);
        //enemyMonster.GetComponent<EnemyMonster>().Init();
    }

    void Update()
    {
        
    }
}
