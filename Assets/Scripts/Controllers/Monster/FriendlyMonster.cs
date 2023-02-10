using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public abstract class FriendlyMonster : Monster
{
    public override void Init()
    {
        base.Init();

        towerPosition = Managers.Game.enemyTower.transform.position;

        waypoints = Managers.Game.enemyTower.GetComponent<EnemyTowerController>().LstWayPoint;

        currentWayPointIndex = 0;
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

    protected override bool CheckAttackCollisionTagname(string collder_tag)
    {
        if (collder_tag == Define.TagName.FriendlyProjectile.ToString())
            return false;

        return true;
    }

    public override void OnDead()
    {
        base.OnDead();

        Managers.Game.Despawn(Define.ObjectType.FriendlyMeleeMonster, gameObject);
    }
}
