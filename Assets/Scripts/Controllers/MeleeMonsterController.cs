using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeMonsterController : MonoBehaviour
{
    // Stats
    public float _hp = 50f;
    public float _sightRange = 10f;
    public float _attackRange = 3f;
    public float _attackSpeed = 0.7f;
    public float _maxChaseDistance = 10f;
    protected float _attackPower;
    float _curHP;

    // AI
    protected NavMeshAgent _navMeshAgent;
    protected Transform[] _waypoints;
    protected int _currentWayPointIndex;
    float _targetChaseDistance;
    Vector3 _beforePosition;

    // Attack
    bool isAttackReady;
    float _attackTimer;
    protected GameObject _attackTarget;
    protected Vector3 _towerPosition;

    // State
    enum MonsterState { Idle, Walking, Chasing, Attacking, Returning, Dead };
    MonsterState currentMonsterState;

    Rigidbody _r;
    protected Animator _anim;
    enum Anims { None, Idle, Walk, Attack, Dead };
    Anims currentAnim;

    public void Init()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _r = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();

        _waypoints = null;
        _currentWayPointIndex = 0;
        _beforePosition = Vector3.zero;
        isAttackReady = true;
        _attackTimer = 0f;
        _attackTarget = null;
        _towerPosition = Vector3.zero;

        currentMonsterState = MonsterState.Idle;
        currentAnim = Anims.None;
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        if (currentMonsterState != MonsterState.Dead)
        {
            AILogic();
        }
    }

    void AILogic()
    {
        switch (currentMonsterState)
        {
            case MonsterState.Idle:
                if (currentAnim != Anims.Idle)
                {
                    _anim.CrossFade("Idle", 0.2f);
                    currentAnim = Anims.Idle;
                }

                // 길 찾기
                _navMeshAgent.isStopped = false;
                _attackTarget = FindAttackTarget();

                if (_attackTarget)
                {
                    _beforePosition = transform.position;
                    currentMonsterState = MonsterState.Chasing;
                    break;
                }

                // 다음 웨이포인트로 이동
                if (_currentWayPointIndex < _waypoints.Length - 1)
                {
                    _currentWayPointIndex++;
                    _navMeshAgent.SetDestination(_waypoints[_currentWayPointIndex].position);
                    currentMonsterState = MonsterState.Walking;
                }
                // 끝까지 갔다면 무조건 타워로 이동
                else
                {
                    //_navMeshAgent.SetDestination(Managers.Stage.EnemyTower.transform.position);
                    currentMonsterState = MonsterState.Walking;
                }

                break;
            case MonsterState.Walking:
                if (currentAnim != Anims.Walk)
                {
                    _anim.CrossFade("Walk", 0.2f);
                    currentAnim = Anims.Walk;
                }

                // 공격 대상 찾기

                break;
            case MonsterState.Chasing:
                // 추격
                break;
            case MonsterState.Attacking:
                // 공격
                break;
            case MonsterState.Returning:
                // 추격했던 길 다시 돌아가기
                break;
        }
    }

    GameObject FindAttackTarget()
    {
        return null;
    }
}
