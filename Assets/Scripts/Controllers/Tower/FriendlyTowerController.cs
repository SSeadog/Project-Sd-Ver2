using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyTowerController : TowerBase
{
    public override void Init()
    {
        base.Init();

        _type = Define.ObjectType.FriendlyTower;
        Managers.Game.FriendlyTower = gameObject;
    }

    public override void OnDead()
    {
        Managers.Game.GameLose();
    }
}
