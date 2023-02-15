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

        _towerPosition = Managers.Game.friendlyTower.transform.position;

        _waypoints = Managers.Game.friendlyTower.GetComponent<FriendlyTowerController>().LstWayPoint;
        
        _currentWayPointIndex = 0;
    }

    protected override GameObject FindAttackTarget()
    {
        GameObject minDistanceGameObject = null;

        float minDistance = 9999999f;

        // 플레이어 몬스터 중에서 거리가 가장 가까운 녀석 찾기
        for (int i = 0; i < Managers.Game.friendlyMonsters.Count; i++)
        {
            if (Vector3.Distance(transform.position, Managers.Game.friendlyMonsters[i].transform.position) - 1f < minDistance)
            {
                minDistance = Vector3.Distance(transform.position, Managers.Game.friendlyMonsters[i].transform.position) - 1f;
                minDistanceGameObject = Managers.Game.friendlyMonsters[i];
                _curAttackTargetType = AttackTargetType.Monster;
                _curTargetSize = 1f;
            }
        }

        // 플레이어가 거리가 더 가까운지 확인
        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        if (Vector3.Distance(transform.position, playerPosition) - 1f < minDistance)
        {
            minDistance = Vector3.Distance(transform.position, playerPosition) - 1f;
            minDistanceGameObject = GameObject.FindGameObjectWithTag("Player");
            _curAttackTargetType = AttackTargetType.Monster;
            _curTargetSize = 1f;
        }

        // 플레이어 타워가 거리가 더 가까운지 확인
        if (Vector3.Distance(transform.position, _towerPosition) - 14f < minDistance)
        {
            minDistance = Vector3.Distance(transform.position, _towerPosition) - 14f;
            minDistanceGameObject = Managers.Game.friendlyTower;
            _curAttackTargetType = AttackTargetType.Tower;
            _curTargetSize = 14f;
        }

        // 가장 가까운 공격대상이 시야 밖에 있다면 
        if (_stat.SightRange < minDistance)
        {
            return null;
        }

        return minDistanceGameObject;
    }

    protected override bool CheckTeamTagname(string collider_tag)
    {
        if (collider_tag == Define.TagName.EnemyProjectile.ToString())
            return false;

        if (collider_tag == Define.TagName.EnemyMonster.ToString())
            return false;

        return true;
    }

    public override void OnDead()
    {
        base.OnDead();

        Managers.Game.Despawn(Define.ObjectType.EnemyMeleeMonster, gameObject);
    }
}
