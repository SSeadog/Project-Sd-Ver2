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

        _towerPosition = Managers.Game.EnemyTower.transform.position;

        _waypoints = Managers.Game.EnemyTower.GetComponent<EnemyTowerController>().LstWayPoint;

        _moveDeg = 0;
    }

    protected override GameObject FindMinDistAttackTarget()
    {
        GameObject minDistanceGameObject = null;

        float minDistance = 9999999f;

        // 적 몬스터 중에서 거리가 가장 가까운 녀석 찾기
        for (int i = 0; i < Managers.Game.EnemyMonsters.Count; i++)
        {
            if (Vector3.Distance(transform.position, Managers.Game.EnemyMonsters[i].transform.position) - 1f < minDistance)
            {
                minDistance = Vector3.Distance(transform.position, Managers.Game.EnemyMonsters[i].transform.position) - 1f;
                minDistanceGameObject = Managers.Game.EnemyMonsters[i];
            }
        }

        // 적 타워가 거리가 더 가까운지 확인
        if (Vector3.Distance(transform.position, _towerPosition) - 14f < minDistance)
        {
            minDistance = Vector3.Distance(transform.position, _towerPosition) - 14f;
            minDistanceGameObject = Managers.Game.EnemyTower;
        }

        // 가장 가까운 공격대상이 시야 밖에 있다면
        if (_stat.SightRange < minDistance)
        {
            return null;
        }

        _curTargetSize = minDistanceGameObject.GetComponent<Stat>().Size;

        return minDistanceGameObject;
    }

    protected override bool CheckTeamTagname(string collder_tag)
    {
        if (collder_tag == Define.TagName.Player.ToString())
            return false;

        if (collder_tag == Define.TagName.FriendlyProjectile.ToString())
            return false;

        if (collder_tag == Define.TagName.FriendlyMonster.ToString())
            return false;

        return true;
    }

    public override void OnDead()
    {
        base.OnDead();

        Managers.Game.killedFriendlyMonsterCount++;
        Managers.Game.Despawn(Define.ObjectType.FriendlyMeleeMonster, gameObject);
    }
}
