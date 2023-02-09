using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMonsterSpawn : MonoBehaviour
{
    #region Monsters
    [SerializeField]
    GameObject enemyMonster1;
    [SerializeField]
    GameObject enemyMonster2;
    [SerializeField]
    GameObject enemyMonster3;
    #endregion

    int counter = 0;

    void Init()
    {

    }

    void Start()
    {
        Init();
    }

    public void SpawnMeleeMonster()
    {
        SpawnMonster(enemyMonster1);
    }

    public void SpawnRangeMonster()
    {
        SpawnMonster(enemyMonster2);
    }

    public void SpawnTankerMonster()
    {
        SpawnMonster(enemyMonster3);
    }

    public void SpawnMonster(GameObject original)
    {
        //GameObject monster1 = Instantiate(original, Managers.Stage.EnemyMonsterRoot.transform.position, Quaternion.identity);
        //monster1.transform.SetParent(Managers.Stage.EnemyMonsterRoot.transform, true);
        //monster1.name += counter;

        //Managers.Stage.EnemyMonsters.Add(monster1);

        //warSituationUIScript.AddEnemyMonsterPoint(counter);
        //counter++;
    }
}
