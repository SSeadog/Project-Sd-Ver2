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
    protected NavMeshAgent _navMeshAgent;
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
    enum MonsterState
    {
        Idle,
        Walking,
        LookingForPath,
        Chasing,
        Attacking,
        Returning,
        Dead
    };
    MonsterState currentMonsterState = MonsterState.Idle;

    Rigidbody r;
    
    protected Animator _anim;
    protected enum Anims
    {
        Idle,
        Walk,
        Attack,
        Dead
    }
    protected Anims _curAnim = Anims.Idle;

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
        _navMeshAgent = GetComponent<NavMeshAgent>();
        r = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();

        _stat.OnDeadAction += OnDead;
    }

    void Update()
    {
        if (currentMonsterState != MonsterState.Dead)
        {
            //  AI 실행 주기
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
                PlayAnim(Anims.Idle);
                currentMonsterState = MonsterState.LookingForPath;

                break;
            case MonsterState.Walking:
                PlayAnim(Anims.Walk);
                attackTarget = FindAttackTarget();

                if (attackTarget)
                {
                    beforePosition = transform.position;
                    currentMonsterState = MonsterState.Chasing;
                }
                else if (!attackTarget && currentWayPointIndex < waypoints.Count && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                {
                    currentMonsterState = MonsterState.LookingForPath;
                    break;
                }
                else if (!attackTarget)
                {
                    _navMeshAgent.SetDestination(waypoints[currentWayPointIndex].position);
                    break;
                }

                break;
            case MonsterState.LookingForPath:
                PlayAnim(Anims.Idle);
                _navMeshAgent.isStopped = false;
                attackTarget = FindAttackTarget();

                if (attackTarget)
                {
                    beforePosition = transform.position;
                    currentMonsterState = MonsterState.Chasing;
                    break;
                }

                // 다음 웨이포인트로 이동
                if (currentWayPointIndex < waypoints.Count - 1)
                {
                    currentWayPointIndex += 1;
                    _navMeshAgent.SetDestination(waypoints[currentWayPointIndex].position);
                    currentMonsterState = MonsterState.Walking;
                }
                // 끝까지 갔다면 무조건 타워로 이동
                else
                {
                    _navMeshAgent.SetDestination(Managers.Game.enemyTower.transform.position);
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

                // attackTarget이 있고 attackRange 밖에 있다면 추격 거리 기록
                else if (attackTarget && Vector3.Distance(transform.position, attackTarget.transform.position) - curTargetSize > _stat.AttackRange)
                {
                    PlayAnim(Anims.Walk);

                    _navMeshAgent.isStopped = false;
                    _navMeshAgent.SetDestination(attackTarget.transform.position);
                    targetChaseDistance += Vector3.Distance(transform.position, beforePosition);
                    beforePosition = transform.position;

                    // 최대 추격 거리만큼 쫓아갔다면 다시 원래 루트로 돌아오기
                    if (targetChaseDistance > _stat.MaxChaseDistance)
                    {
                        attackTarget = null;
                        targetChaseDistance = 0f;

                        UpdateWayPoint();
                        _navMeshAgent.SetDestination(waypoints[currentWayPointIndex].position);

                        currentMonsterState = MonsterState.Returning;
                    }
                }
                // attackTarget이 있고 attackRange 안에 있다면 공격 상태로 변경
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

                PlayAnim(Anims.Attack);
                _navMeshAgent.isStopped = true;

                // 공격 대상이 시야 안에 있다면
                if (_navMeshAgent.remainingDistance - curTargetSize < _stat.SightRange)
                {
                    // 공격 대상이 공격 범위 밖에 있다면
                    if (Vector3.Distance(transform.position, attackTarget.transform.position) - curTargetSize > _stat.AttackRange)
                    {
                        beforePosition = transform.position;
                        currentMonsterState = MonsterState.Chasing;
                        break;
                    }

                    FaceTarget();

                    // 공격 준비가 됐다면 공격
                    if (isAttackReady)
                    {
                        AttckTarget();
                        isAttackReady = false;
                    }
                    // 공격 준비가 안됐으면 attackSpeed까지 대기
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
                // 공격 대상이 시야 밖에 있다면
                else
                {
                    attackTarget = null;
                    UpdateWayPoint();
                    currentMonsterState = MonsterState.LookingForPath;
                }

                break;
            case MonsterState.Returning:
                if (_navMeshAgent.remainingDistance < _navMeshAgent.stoppingDistance)
                {
                    currentMonsterState = MonsterState.LookingForPath;
                }

                PlayAnim(Anims.Walk);

                break;
        }
    }

    void PlayAnim(Anims animation)
    {
        if (_curAnim == animation)
            return;

        _curAnim = animation;
        _anim.CrossFade(_curAnim.ToString(), 0.1f);
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
        // 현재 위치보다 z가 작은 웨이포인트로 업데이트
        for (int i = currentWayPointIndex; i < waypoints.Count - 1; i++)
        {
            if (transform.position.z < waypoints[i].position.z)
            {
                currentWayPointIndex = i;
                break;
            }
        }
    }

    public virtual void OnDead()
    {
        PlayAnim(Anims.Dead);
        currentMonsterState = MonsterState.Dead;

        GetComponent<CapsuleCollider>().enabled = false;
        _navMeshAgent.isStopped = true;
    }

    protected virtual bool CheckAttackCollisionTagname(string collder_tag)
    {
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (!CheckAttackCollisionTagname(other.tag))
        //    return;

        Stat attackerStat = other.GetComponent<Stat>();
        _stat.OnAttacked(attackerStat);
    }
}
