using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMonsterSworder : EnemyMonster
{
    protected override void AttckTarget()
    {
        // Animation Event�� �����Ͽ� ���� ���� �ʿ�x
    }

    public void OnAttackEvent()
    {
        if (_attackTarget == null)
            return;

        _attackTarget.GetComponent<Stat>().GetAttacked(_stat);
    }
}
