using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyMonsterArcher : EnemyMonster
{
    public GameObject arrow;
    public Transform arrowPosition;

    protected override void AttckTarget()
    {
        // Anim 따라서 실행되기에 따로 뭔가 할 필요 없음
    }

    public void FireArrow()
    {
        // 활 쏘기
        GameObject instanceArrow = Instantiate(arrow, arrowPosition.position, transform.rotation * Quaternion.Euler(90f, 0, 0));
        Rigidbody arrowRigid = instanceArrow.GetComponent<Rigidbody>();

        arrowRigid.AddForce((_attackTarget.transform.position - transform.position).normalized * 2000f);
    }

    public void EndFireArrow()
    {
        PlayAnim(Anims.Idle);
    }
}
