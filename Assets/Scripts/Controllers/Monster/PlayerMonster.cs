using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public abstract class PlayerMonster : Monster
{
    private void Start()
    {
        base.Init();

        towerPosition = Managers.Game.enemyTower.transform.position;

        waypoints = Managers.Game.enemyTower.GetComponent<EnemyTowerController>().LstWayPoint;

        currentWayPointIndex = 0;
        navMeshAgent.SetDestination(waypoints[currentWayPointIndex].position);
    }

    protected override GameObject FindAttackTarget()
    {
        GameObject minDistanceGameObject = null;

        float minDistance = 9999999f;

        // 적 몬스터 중에서 거리가 가장 가까운 녀석 찾기
        for (int i = 0; i < Managers.Game.enemyMonsters.Count; i++)
        {
            if (Vector3.Distance(transform.position, Managers.Game.enemyMonsters[i].transform.position) - 1f < minDistance)
            {
                minDistance = Vector3.Distance(transform.position, Managers.Game.enemyMonsters[i].transform.position) - 1f;
                minDistanceGameObject = Managers.Game.enemyMonsters[i];
                curAttackTargetType = AttackTargetType.Monster;
                curTargetSize = 1f;
            }
        }

        // 적 타워가 거리가 더 가까운지 확인
        if (Vector3.Distance(transform.position, towerPosition) - 14f < minDistance)
        {
            minDistance = Vector3.Distance(transform.position, towerPosition) - 14f;
            minDistanceGameObject = Managers.Game.enemyTower;
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
        for (int i = 0; i < Managers.Game.playerMonsters.Count; i++)
        {
            if (name == Managers.Game.playerMonsters[i].name)
                Managers.Game.playerMonsters.RemoveAt(i);
        }
    }

    protected override bool CheckAttackCollisionTagname(string collder_tag)
    {
        if (collder_tag == "Ground" || collder_tag == "PlayerProjectile" || collder_tag == "PlayerMonster" || collder_tag == "EnemyMonster" || collder_tag == "Player" || collder_tag == "Tower")
            return false;

        return true;
    }
}
