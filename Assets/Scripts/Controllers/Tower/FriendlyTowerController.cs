using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyTowerController : TowerBase
{
    public override void Init(Define.ObjectType type)
    {
        base.Init(type);
        
        Managers.Game.friendlyTower = gameObject;
    }

    public override void OnDead()
    {
        Managers.Game.GameLose();
    }
}
