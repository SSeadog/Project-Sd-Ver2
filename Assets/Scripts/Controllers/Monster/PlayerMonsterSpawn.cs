using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMonsterSpawn : MonoBehaviour
{
    #region playerMonster
    [SerializeField]
    GameObject playerMonster1;
    [SerializeField]
    GameObject playerMonster2;
    [SerializeField]
    GameObject playerMonster3;
    #endregion

    public GameObject MonsterRoot;

    PlayerTowerController playerTower;

    int counter = 0;

    float meleeMonsterPoint = 10f;
    float rangeMonsterPoint = 15f;
    float tankerMonsterPoint = 30f;

    void Init()
    {
        playerTower = Managers.Game.playerTower.GetComponent<PlayerTowerController>();
    }

    private void Start()
    {
        Init();
    }

    void Update()
    {
        _playerMonsterSpawn();
    }

    void _playerMonsterSpawn()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    if (playerTower.GetCurResourcePoint() > meleeMonsterPoint)
        //    {
        //        playerTower.UseResourcePoint(meleeMonsterPoint);
        //        spawnMonster(playerMonster1);
        //        counter++;
        //    }
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    if (playerTower.GetCurResourcePoint() > rangeMonsterPoint)
        //    {
        //        playerTower.UseResourcePoint(rangeMonsterPoint);
        //        spawnMonster(playerMonster2);
        //        counter++;
        //    }
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    if (playerTower.GetCurResourcePoint() > tankerMonsterPoint)
        //    {
        //        playerTower.UseResourcePoint(tankerMonsterPoint);
        //        spawnMonster(playerMonster3);
        //        counter++;
        //    }
        //}
    }

    void spawnMonster(GameObject go)
    {
        //GameObject monster1 = Instantiate(go, Managers.Game.PlayerMonsterRoot.transform.position, Quaternion.identity);
        //monster1.transform.SetParent(Managers.Game.PlayerMonsterRoot.transform, true);
        //monster1.name += counter;

        //Managers.Game.PlayerMonsters.Add(monster1);

        //warSituationUIScript.AddPlayerMonsterPoint(counter);
    }
}
