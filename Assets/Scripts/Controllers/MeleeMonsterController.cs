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

                // �� ã��
                _navMeshAgent.isStopped = false;
                _attackTarget = FindAttackTarget();

                if (_attackTarget)
                {
                    _beforePosition = transform.position;
                    currentMonsterState = MonsterState.Chasing;
                    break;
                }

                // ���� ��������Ʈ�� �̵�
                if (_currentWayPointIndex < _waypoints.Length - 1)
                {
                    _currentWayPointIndex++;
                    _navMeshAgent.SetDestination(_waypoints[_currentWayPointIndex].position);
                    currentMonsterState = MonsterState.Walking;
                }
                // ������ ���ٸ� ������ Ÿ���� �̵�
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

                // ���� ��� ã��

                break;
            case MonsterState.Chasing:
                // �߰�
                break;
            case MonsterState.Attacking:
                // ����
                break;
            case MonsterState.Returning:
                // �߰��ߴ� �� �ٽ� ���ư���
                break;
        }
    }

    GameObject FindAttackTarget()
    {
        return null;
    }
}
