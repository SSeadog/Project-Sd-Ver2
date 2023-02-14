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
    public float _brainSpeed = 0.5f;
    protected List<Transform> _waypoints;
    protected int _currentWayPointIndex;
    float _brainTimer;

    // Attack
    bool _isAttackReady = true;
    float _currentAttackTime = 0f;
    protected GameObject _attackTarget;
    protected enum AttackTargetType { Monster, Tower };
    protected AttackTargetType _curAttackTargetType;
    protected float _curTargetSize = 0f;
    protected Vector3 _towerPosition;

    float _targetChaseDistance = 0f;
    Vector3 _beforePosition;

    // State
    enum MonsterState
    {
        Idle,
        Walking,
        LookingForPath,
        Chasing,
        Attacking,
        Returning,
        Stiffing,
        Dead
    };
    MonsterState _currentMonsterState = MonsterState.Idle;

    Rigidbody _r;
    
    protected Animator _anim;
    protected enum Anims
    {
        Idle,
        Walk,
        Attack,
        Dead
    }
    protected Anims _curAnim = Anims.Idle;


    public virtual void Init()
    {

    }

    void Start()
    {
        Init();

        _stat = GetComponent<MonsterStat>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _r = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();

        _stat.OnAttacktedAction += OnAttacked;
        _stat.OnDeadAction += OnDead;
        _navMeshAgent.stoppingDistance = _stat.AttackRange;
        _navMeshAgent.speed = _stat.Speed;
    }

    void Update()
    {
        if (_currentMonsterState != MonsterState.Dead)
        {
            //  AI ���� �ֱ�
            _brainTimer += Time.deltaTime;
            if (_brainTimer >= _brainSpeed)
            {
                AILogic();

                _brainTimer = 0;
            }
        }
    }

    void AILogic()
    {
        if (_towerPosition == null)
            return;

        switch (_currentMonsterState)
        {
            case MonsterState.Idle:
                PlayAnim(Anims.Idle);
                _currentMonsterState = MonsterState.LookingForPath;

                break;
            case MonsterState.Walking:
                PlayAnim(Anims.Walk);
                _attackTarget = FindAttackTarget();

                if (_attackTarget)
                {
                    _beforePosition = transform.position;
                    _currentMonsterState = MonsterState.Chasing;
                }
                else if (!_attackTarget && _currentWayPointIndex < _waypoints.Count && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                {
                    _currentMonsterState = MonsterState.LookingForPath;
                    break;
                }
                else if (!_attackTarget)
                {
                    _navMeshAgent.SetDestination(_waypoints[_currentWayPointIndex].position);
                    break;
                }

                break;
            case MonsterState.LookingForPath:
                PlayAnim(Anims.Idle);
                _navMeshAgent.isStopped = false;
                _attackTarget = FindAttackTarget();

                if (_attackTarget)
                {
                    _beforePosition = transform.position;
                    _currentMonsterState = MonsterState.Chasing;
                    break;
                }

                // ���� ��������Ʈ�� �̵�
                if (_currentWayPointIndex < _waypoints.Count - 1)
                {
                    _currentWayPointIndex += 1;
                    _navMeshAgent.SetDestination(_waypoints[_currentWayPointIndex].position);
                    _currentMonsterState = MonsterState.Walking;
                }
                // ������ ���ٸ� ������ Ÿ���� �̵�
                else
                {
                    _navMeshAgent.SetDestination(Managers.Game.enemyTower.transform.position);
                    _currentMonsterState = MonsterState.Walking;
                }

                break;
            case MonsterState.Chasing:
                if (!_attackTarget)
                {
                    _attackTarget = null;
                    _currentMonsterState = MonsterState.LookingForPath;
                    break;
                }

                // attackTarget�� �ְ� attackRange �ۿ� �ִٸ� �߰� �Ÿ� ���
                else if (_attackTarget && Vector3.Distance(transform.position, _attackTarget.transform.position) - _curTargetSize > _stat.AttackRange)
                {
                    PlayAnim(Anims.Walk);

                    _navMeshAgent.isStopped = false;
                    _navMeshAgent.SetDestination(_attackTarget.transform.position);
                    _targetChaseDistance += Vector3.Distance(transform.position, _beforePosition);
                    _beforePosition = transform.position;

                    // �ִ� �߰� �Ÿ���ŭ �Ѿư��ٸ� �ٽ� ���� ��Ʈ�� ���ƿ���
                    if (_targetChaseDistance > _stat.MaxChaseDistance)
                    {
                        _attackTarget = null;
                        _targetChaseDistance = 0f;

                        UpdateWayPoint();
                        _navMeshAgent.SetDestination(_waypoints[_currentWayPointIndex].position);

                        _currentMonsterState = MonsterState.Returning;
                    }
                }
                // attackTarget�� �ְ� attackRange �ȿ� �ִٸ� ���� ���·� ����
                else if (_attackTarget && Vector3.Distance(transform.position, _attackTarget.transform.position) - _curTargetSize < _stat.AttackRange)
                {
                    _currentMonsterState = MonsterState.Attacking;
                }

                break;
            case MonsterState.Attacking:
                if (!_attackTarget)
                {
                    _attackTarget = null;
                    UpdateWayPoint();
                    _currentMonsterState = MonsterState.LookingForPath;
                    break;
                }

                PlayAnim(Anims.Attack);
                _navMeshAgent.isStopped = true;

                // ���� ����� �þ� �ȿ� �ִٸ�
                if (_navMeshAgent.remainingDistance - _curTargetSize < _stat.SightRange)
                {
                    // ���� ����� ���� ���� �ۿ� �ִٸ�
                    if (Vector3.Distance(transform.position, _attackTarget.transform.position) - _curTargetSize > _stat.AttackRange)
                    {
                        _beforePosition = transform.position;
                        _currentMonsterState = MonsterState.Chasing;
                        break;
                    }

                    FaceTarget();

                    // ���� �غ� �ƴٸ� ����
                    if (_isAttackReady)
                    {
                        AttckTarget();
                        _isAttackReady = false;
                    }
                    // ���� �غ� �ȵ����� attackSpeed���� ���
                    else
                    {
                        _currentAttackTime += _brainTimer;
                        if (_currentAttackTime >= _stat.AttackSpeed)
                        {
                            _isAttackReady = true;
                            _currentAttackTime = 0f;
                        }
                    }

                }
                // ���� ����� �þ� �ۿ� �ִٸ�
                else
                {
                    _attackTarget = null;
                    UpdateWayPoint();
                    _currentMonsterState = MonsterState.LookingForPath;
                }

                break;
            case MonsterState.Returning:
                if (_navMeshAgent.remainingDistance < _navMeshAgent.stoppingDistance)
                {
                    _currentMonsterState = MonsterState.LookingForPath;
                }

                PlayAnim(Anims.Walk);

                break;
            case MonsterState.Stiffing:
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
        Vector3 direction = (_attackTarget.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    protected abstract void AttckTarget();

    protected abstract GameObject FindAttackTarget();

    void UpdateWayPoint()
    {
        // ���� ��ġ���� z�� ���� ��������Ʈ�� ������Ʈ
        for (int i = _currentWayPointIndex; i < _waypoints.Count - 1; i++)
        {
            if (transform.position.z < _waypoints[i].position.z)
            {
                _currentWayPointIndex = i;
                break;
            }
        }
    }

    public virtual void OnAttacked(float stiffingTime = 0f)
    {
        if (stiffingTime == 0f)
        {
            return;
        }
        
        StartCoroutine(CoStiff(stiffingTime));
        StartCoroutine(CoChangeColor(stiffingTime));
    }

    IEnumerator CoStiff(float time)
    {
        _currentMonsterState = MonsterState.Stiffing;
        _navMeshAgent.speed = 0f;
        _navMeshAgent.velocity = Vector3.zero;
        _anim.speed = 0f;
        
        yield return new WaitForSeconds(time);

        _currentMonsterState = MonsterState.Idle;
        _navMeshAgent.speed = _stat.Speed;
        _anim.speed = 1f;
    }

    IEnumerator CoChangeColor(float time)
    {
        MeshRenderer[] mrs = gameObject.GetComponentsInChildren<MeshRenderer>();

        List<List<Color>> colors = new List<List<Color>>();

        for (int i = 0; i < mrs.Length; i++)
        {
            Material[] mats = mrs[i].materials;
            colors.Add(new List<Color>());

            for (int j = 0; j < mats.Length; j++)
            {
                colors[i].Add(mats[j].color);
                mats[j].color = Color.red;
            }
        }

        yield return new WaitForSeconds(time);

        for (int i = 0; i < mrs.Length; i++)
        {
            Material[] mats = mrs[i].materials;
            colors.Add(new List<Color>());

            for (int j = 0; j < mats.Length; j++)
            {
                mats[j].color = colors[i][j];
            }
        }
    }

    public virtual void OnDead()
    {
        PlayAnim(Anims.Dead);
        _currentMonsterState = MonsterState.Dead;

        GetComponent<CapsuleCollider>().enabled = false;
        _navMeshAgent.isStopped = true;
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
        _stat.GetAttacked(attackerStat);
    }
}
