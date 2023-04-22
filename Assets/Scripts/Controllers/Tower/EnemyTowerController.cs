using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTowerController : TowerBase
{
    public override void Init()
    {
        base.Init();

        _type = Define.ObjectType.EnemyTower;
        Managers.Game.EnemyTower = gameObject;
    }

    public override void OnDead()
    {
        Managers.Game.GameWin();
    }
}
