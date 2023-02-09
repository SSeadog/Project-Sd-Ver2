using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public abstract class EnemyMonster : Monster
{
    public override void Init()
    {
        base.Init();

        towerPosition = Managers.Game.playerTower.transform.position;

        waypoints = Managers.Game.playerTower.GetComponent<PlayerTowerController>().LstWayPoint;
        
        currentWayPointIndex = 0;
    }

    protected override GameObject FindAttackTarget()
    {
        GameObject minDistanceGameObject = null;

        float minDistance = 9999999f;

        // 플레이어 몬스터 중에서 거리가 가장 가까운 녀석 찾기
        for (int i = 0; i < Managers.Game.playerMonsters.Count; i++)
        {
            if (Vector3.Distance(transform.position, Managers.Game.playerMonsters[i].transform.position) - 1f < minDistance)
            {
                minDistance = Vector3.Distance(transform.position, Managers.Game.playerMonsters[i].transform.position) - 1f;
                minDistanceGameObject = Managers.Game.playerMonsters[i];
                curAttackTargetType = AttackTargetType.Monster;
                curTargetSize = 1f;
            }
        }

        // 플레이어가 거리가 더 가까운지 확인
        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        if (Vector3.Distance(transform.position, playerPosition) - 1f < minDistance)
        {
            minDistance = Vector3.Distance(transform.position, playerPosition) - 1f;
            minDistanceGameObject = GameObject.FindGameObjectWithTag("Player");
            curAttackTargetType = AttackTargetType.Monster;
            curTargetSize = 1f;
        }

        // 플레이어 타워가 거리가 더 가까운지 확인
        if (Vector3.Distance(transform.position, towerPosition) - 14f < minDistance)
        {
            minDistance = Vector3.Distance(transform.position, towerPosition) - 14f;
            minDistanceGameObject = Managers.Game.playerTower;
            curAttackTargetType = AttackTargetType.Tower;
            curTargetSize = 14f;
        }

        // 가장 가까운 공격대상이 시야 밖에 있다면 
        if (_stat.SightRange < minDistance)
        {
            return null;
        }

        return minDistanceGameObject;
    }

    protected override void OnRemoveThis()
    {
        for (int i = 0; i < Managers.Game.enemyMonsters.Count; i++)
        {
            if (name == Managers.Game.enemyMonsters[i].name)
                Managers.Game.enemyMonsters.RemoveAt(i);
        }
    }

    protected override bool CheckAttackCollisionTagname(string collder_tag)
    {
        if (collder_tag == "Ground" || collder_tag == "EnemyMonster" || collder_tag == "PlayerMonster" || collder_tag == "Player" || collder_tag == "EnemyProjectile")
            return false;

        return true;
    }
}
