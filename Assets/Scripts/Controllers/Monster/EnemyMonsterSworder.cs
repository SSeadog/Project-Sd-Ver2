using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMonsterSworder : EnemyMonster
{
    protected override void AttckTarget()
    {
        // Animation Event로 실행하여 따로 동작 필요x
    }

    public void OnAttackEvent()
    {
        if (_attackTarget == null)
            return;

        _attackTarget.GetComponent<Stat>().GetAttacked(_stat);
    }
}
