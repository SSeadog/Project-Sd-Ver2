using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public abstract class Monster : MonoBehaviour
{
    protected MonsterStat _stat;

    // AI
    protected NavMeshAgent navMeshAgent;
    public float brainSpeed = 0.5f;
    protected List<Transform> waypoints;
    protected int currentWayPointIndex;
    float brainTimer;

    // Attack
    bool isAttackReady = true;
    float currentAttackTime = 0f;
    protected GameObject attackTarget;
    protected enum AttackTargetType { Monster, Tower };
    protected AttackTargetType curAttackTargetType;
    protected float curTargetSize = 0f;
    protected Vector3 towerPosition;

    float targetChaseDistance = 0f;
    Vector3 beforePosition;

    // State
    enum MonsterState { Idle, Walking, LookingForPath, Chasing, Attacking, Returning, Dead };
    MonsterState currentMonsterState = MonsterState.Idle;

    Rigidbody r;
    protected Animator anim;
    //public Image hpBar;

    public virtual void Init()
    {
        //InitHpBarSize();
    }

    void InitHpBarSize()
    {
        //hpBar.rectTransform.localScale = new Vector3(1f, 1f, 1f);
    }

    void Start()
    {
        Init();

        _stat = GetComponent<MonsterStat>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        r = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (currentMonsterState != MonsterState.Dead)
        {
            //  AI ���� �ֱ�
            brainTimer += Time.deltaTime;
            if (brainTimer >= brainSpeed)
            {
                AILogic();

                brainTimer = 0;
            }
        }
    }

    void AILogic()
    {
        if (towerPosition == null)
            return;

        switch (currentMonsterState)
        {
            case MonsterState.Idle:
                anim.SetBool("IsWalk", false);
                currentMonsterState = MonsterState.LookingForPath;

                break;
            case MonsterState.Walking:
                anim.SetBool("IsWalk", true);
                attackTarget = FindAttackTarget();

                if (attackTarget)
                {
                    beforePosition = transform.position;
                    currentMonsterState = MonsterState.Chasing;
                }
                else if (!attackTarget && currentWayPointIndex < waypoints.Count && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                {
                    currentMonsterState = MonsterState.LookingForPath;
                    break;
                }
                else if (!attackTarget)
                {
                    navMeshAgent.SetDestination(waypoints[currentWayPointIndex].position);
                    break;
                }

                break;
            case MonsterState.LookingForPath:
                navMeshAgent.isStopped = false;
                anim.SetBool("IsWalk", true);
                attackTarget = FindAttackTarget();

                if (attackTarget)
                {
                    beforePosition = transform.position;
                    currentMonsterState = MonsterState.Chasing;
                    break;
                }

                // ���� ��������Ʈ�� �̵�
                if (currentWayPointIndex < waypoints.Count - 1)
                {
                    currentWayPointIndex += 1;
                    navMeshAgent.SetDestination(waypoints[currentWayPointIndex].position);
                    currentMonsterState = MonsterState.Walking;
                }
                // ������ ���ٸ� ������ Ÿ���� �̵�
                else
                {
                    navMeshAgent.SetDestination(Managers.Game.enemyTower.transform.position);
                    currentMonsterState = MonsterState.Walking;
                }

                break;
            case MonsterState.Chasing:
                if (!attackTarget)
                {
                    attackTarget = null;
                    currentMonsterState = MonsterState.LookingForPath;
                    break;
                }

                // attackTarget�� �ְ� attackRange �ۿ� �ִٸ� �߰� �Ÿ� ���
                else if (attackTarget && Vector3.Distance(transform.position, attackTarget.transform.position) - curTargetSize > _stat.AttackRange)
                {
                    anim.SetBool("IsWalk", true);

                    navMeshAgent.isStopped = false;
                    navMeshAgent.SetDestination(attackTarget.transform.position);
                    targetChaseDistance += Vector3.Distance(transform.position, beforePosition);
                    beforePosition = transform.position;

                    // �ִ� �߰� �Ÿ���ŭ �Ѿư��ٸ� �ٽ� ���� ��Ʈ�� ���ƿ���
                    if (targetChaseDistance > _stat.MaxChaseDistance)
                    {
                        attackTarget = null;
                        targetChaseDistance = 0f;

                        UpdateWayPoint();
                        navMeshAgent.SetDestination(waypoints[currentWayPointIndex].position);

                        currentMonsterState = MonsterState.Returning;
                    }
                }
                // attackTarget�� �ְ� attackRange �ȿ� �ִٸ� ���� ���·� ����
                else if (attackTarget && Vector3.Distance(transform.position, attackTarget.transform.position) - curTargetSize < _stat.AttackRange)
                {
                    currentMonsterState = MonsterState.Attacking;
                }

                break;
            case MonsterState.Attacking:
                if (!attackTarget)
                {
                    attackTarget = null;
                    UpdateWayPoint();
                    currentMonsterState = MonsterState.LookingForPath;
                    break;
                }

                navMeshAgent.isStopped = true;
                anim.SetBool("IsWalk", false);

                // ���� ����� �þ� �ȿ� �ִٸ�
                if (navMeshAgent.remainingDistance - curTargetSize < _stat.SightRange)
                {
                    // ���� ����� ���� ���� �ۿ� �ִٸ�
                    if (Vector3.Distance(transform.position, attackTarget.transform.position) - curTargetSize > _stat.AttackRange)
                    {
                        beforePosition = transform.position;
                        currentMonsterState = MonsterState.Chasing;
                        break;
                    }

                    FaceTarget();

                    // ���� �غ� �ƴٸ� ����
                    if (isAttackReady)
                    {
                        AttckTarget();
                        isAttackReady = false;
                    }
                    // ���� �غ� �ȵ����� attackSpeed���� ���
                    else
                    {
                        currentAttackTime += brainTimer;
                        if (currentAttackTime >= _stat.AttackSpeed)
                        {
                            isAttackReady = true;
                            currentAttackTime = 0f;
                        }
                    }

                }
                // ���� ����� �þ� �ۿ� �ִٸ�
                else
                {
                    attackTarget = null;
                    UpdateWayPoint();
                    currentMonsterState = MonsterState.LookingForPath;
                }

                break;
            case MonsterState.Returning:
                if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
                {
                    currentMonsterState = MonsterState.LookingForPath;
                }

                anim.SetBool("IsWalk", true);

                break;
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (attackTarget.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    protected abstract void AttckTarget();

    protected abstract GameObject FindAttackTarget();

    void UpdateWayPoint()
    {
        // ���� ��ġ���� z�� ���� ��������Ʈ�� ������Ʈ
        for (int i = currentWayPointIndex; i < waypoints.Count - 1; i++)
        {
            if (transform.position.z < waypoints[i].position.z)
            {
                currentWayPointIndex = i;
                break;
            }
        }
    }

    public void OnDamage(int damage = 10)
    {
        if (_stat.Hp <= 0)
            return;

        _stat.Hp = _stat.Hp - damage < 0 ? 0 : _stat.Hp - damage;

        //hpBar.rectTransform.localScale = new Vector3((float)curHP / (float)HP, 1f, 1f);

        if (_stat.Hp <= 0)
            RemoveThis();

    }
    protected abstract void OnRemoveThis();

    public virtual void RemoveThis()
    {
        OnRemoveThis();

        currentMonsterState = MonsterState.Dead;

        GetComponent<CapsuleCollider>().enabled = false;
        navMeshAgent.isStopped = true;
        anim.SetTrigger("Dead");

        Destroy(gameObject, 1f);
    }

    protected virtual bool CheckAttackCollisionTagname(string collder_tag)
    {
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!CheckAttackCollisionTagname(other.tag))
            return;

        Stat attackerStat = other.GetComponent<Stat>();
        int damage = attackerStat.Power;

        OnDamage(damage);
    }
}
