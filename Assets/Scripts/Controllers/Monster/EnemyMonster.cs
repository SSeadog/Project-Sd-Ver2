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

        _towerPosition = Managers.Game.FriendlyTower.transform.position;

        _waypoints = Managers.Game.FriendlyTower.GetComponent<FriendlyTowerController>().LstWayPoint;
        
        _moveDeg = 180;
    }

    protected override GameObject FindMinDistAttackTarget()
    {
        GameObject minDistanceGameObject = null;

        float minDistance = 9999999f;

        // 플레이어 몬스터 중에서 거리가 가장 가까운 녀석 찾기
        for (int i = 0; i < Managers.Game.FriendlyMonsters.Count; i++)
        {
            if (Vector3.Distance(transform.position, Managers.Game.FriendlyMonsters[i].transform.position) - 1f < minDistance)
            {
                minDistance = Vector3.Distance(transform.position, Managers.Game.FriendlyMonsters[i].transform.position) - 1f;
                minDistanceGameObject = Managers.Game.FriendlyMonsters[i];
                _curTargetSize = 1f;
            }
        }

        // 플레이어가 거리가 더 가까운지 확인
        Vector3 playerPosition = Managers.Game.Player.transform.position;
        if (Vector3.Distance(transform.position, playerPosition) - 1f < minDistance)
        {
            minDistance = Vector3.Distance(transform.position, playerPosition) - 1f;
            minDistanceGameObject = Managers.Game.Player;
        }

        // 플레이어 타워가 거리가 더 가까운지 확인
        if (Vector3.Distance(transform.position, _towerPosition) - 14f < minDistance)
        {
            minDistance = Vector3.Distance(transform.position, _towerPosition) - 14f;
            minDistanceGameObject = Managers.Game.FriendlyTower;
        }

        // 가장 가까운 공격대상이 시야 밖에 있다면 
        if (_stat.SightRange < minDistance)
        {
            return null;
        }

        _curTargetSize = minDistanceGameObject.GetComponent<Stat>().Size;

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

        Managers.Game.killedEnemyMonsterCount++;
        Managers.Game.Despawn(Define.ObjectType.EnemyMeleeMonster, gameObject);
    }
}