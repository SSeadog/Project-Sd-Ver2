using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTowerController : TowerBase
{
    public override void Init(Define.ObjectType type)
    {
        base.Init(type);
        
        Managers.Game.enemyTower = gameObject;
    }

    public override void OnDead()
    {
        Managers.Game.GameWin();
    }
}
