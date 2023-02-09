using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTowerController : TowerBase
{
    public override void Init()
    {
        base.Init();

        Managers.Game.playerTower = gameObject;
    }
}
