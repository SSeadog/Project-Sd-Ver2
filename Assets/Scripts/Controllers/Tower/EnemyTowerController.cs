using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTowerController : TowerBase
{
    public override void Init()
    {
        base.Init();

        Managers.Game.enemyTower = gameObject;
    }
}
