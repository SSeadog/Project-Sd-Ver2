using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using static Define;

// FindMinDistAttackTarget()�� �� update���� �����ϴ� �� �δ��� ���� �� ������
// �׷��� ������ LookingForState, Walk���¿����� ã���� �ߴµ�
// �� ���� ����� ������?
// 1. FindMinDistAttackTarget()�Լ� ��� ����
// 2. ���� �ֱ⿡ �� ���ֱ�

// ���ݻ����� �� ���ߴ� ��� �ְ� ���� ���� ��� �� �ٽ� ������ �� �ֵ��� �����ϴ� �κ� �߰� �ʿ�

public abstract class Monster : MonoBehaviour
{
    protected MonsterStat _stat;
    Rigidbody _r;

    // AI
    protected NavMeshAgent _navMeshAgent;
    protected List<Transform> _waypoints;
    protected int _currentWayPointIndex;
    protected float _moveDeg = 0f;
    float _degGap = 30f;
    float _searchTime = 0.5f;

    // Attack
    protected GameObject _attackTarget;
    protected float _curTargetSize = 0f;
    protected Vector3 _towerPosition;
    bool _isAttackReady = true;
    float _currentAttackTime = 0f;
    
    // Animation
    public enum Anims
    {
        Idle,
        Walk,
        Attack,
        Dead
    }
    protected Anims _curAnim = Anims.Idle;
    protected Animator _anim;

    // State
    MonsterState _state;
    void SetState(MonsterState state)
    {
        if (_state != null)
            _state.OnEnd();

        _state = state;

        if (_state != null)
            _state.OnStart(this);
    }

    public virtual void Init() { }

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

        _currentWayPointIndex = 0;

        SetState(new MonsterIdleState());
    }

    void Update()
    {
        _state?.OnAction();

        if (_isAttackReady == false)
        {
            _currentAttackTime += Time.deltaTime;
            if (_currentAttackTime >= _stat.AttackSpeed)
            {
                _isAttackReady = true;
                _currentAttackTime = 0f;
            }
        }
    }

    float GetDistanceToAttackTarget()
    {
        return Vector3.Distance(transform.position, _attackTarget.transform.position) - _curTargetSize;
    }

    void Move(Vector3 pos)
    {
        if (_navMeshAgent.destination != pos)
            _navMeshAgent.SetDestination(pos);
    }

    Transform FindNextWaypoint()
    {
        // ���� - 3 ~ ���� + 2 ���� �� ���� ��ġ���� ���� ������ ������ ��������Ʈ ã��
        float minDist = 99999f;
        int mindDistWaypointIndex = -1;

        int minIndex = Mathf.Max(0, _currentWayPointIndex - 3);
        int maxIndex = Mathf.Min(_waypoints.Count - 1, _currentWayPointIndex + 3);
        for (int i = minIndex; i < maxIndex; i++)
        {
            if (i == _currentWayPointIndex)
                continue;

            float dist = Vector3.Distance(transform.position, _waypoints[i].position);
            if (dist < minDist)
            {
                // ������ �ѹ� �� üũ
                Vector3 moveVec = (_waypoints[i].position - transform.position).normalized;
                float moveDeg = Mathf.Atan2(moveVec.x, moveVec.z) * Mathf.Rad2Deg;
                if (moveDeg < 0f)
                    moveDeg += 360f;

                float minDeg = (_moveDeg - _degGap < 0f) ? _moveDeg - _degGap + 360f : _moveDeg - _degGap;
                float maxDeg = _moveDeg + _degGap;

                if (minDeg > maxDeg)
                {
                    if ((moveDeg > minDeg && moveDeg < 360f) || (moveDeg > 0f && moveDeg < maxDeg))
                    {
                        minDist = dist;
                        mindDistWaypointIndex = i;
                    }
                }
                else
                {
                    if ((moveDeg > minDeg) && (moveDeg < maxDeg))
                    {
                        minDist = dist;
                        mindDistWaypointIndex = i;
                    }
                }
            }
        }

        if (mindDistWaypointIndex != -1)
        {
            _currentWayPointIndex = mindDistWaypointIndex;
        }
        else if (_currentWayPointIndex < _waypoints.Count - 1)
        {
            _currentWayPointIndex++;
        }

        return _waypoints[_currentWayPointIndex];
    }

    public void PlayAnim(Anims animation)
    {
        if (_curAnim == animation)
            return;

        _curAnim = animation;
        _anim.CrossFade(_curAnim.ToString(), 0.1f);
    }

    void FaceTarget()
    {
        Vector3 direction = (_attackTarget.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
    }

    protected abstract void AttckTarget();

    protected abstract GameObject FindMinDistAttackTarget();

    public virtual void OnAttacked(float stiffingTime = 0f)
    {
        if (stiffingTime == 0f)
        {
            return;
        }

        MonsterStiffingState state = new MonsterStiffingState();
        state.SetBeforeState(_state);
        state.SetStiffTime(0.2f);
        SetState(state);
    }

    public virtual void OnDead()
    {
        SetState(new MonsterDieState());
    }

    protected virtual bool CheckTeamTagname(string collder_tag)
    {
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!CheckTeamTagname(other.tag))
            return;

        Stat attackerStat = other.GetComponent<Stat>();
        _stat.GetAttacked(attackerStat);
    }

    IEnumerator CoChangeColor(float time, Color color)
    {
        SkinnedMeshRenderer[] mrs = GetComponentsInChildren<SkinnedMeshRenderer>();

        List<List<Color>> colors = new List<List<Color>>();

        for (int i = 0; i < mrs.Length; i++)
        {
            Material[] mats = mrs[i].materials;
            colors.Add(new List<Color>());

            for (int j = 0; j < mats.Length; j++)
            {
                colors[i].Add(mats[j].color);
                mats[j].color = color;
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

    class MonsterState
    {
        protected Monster _m;

        public virtual void OnStart(Monster m) { _m = m; }
        public virtual void OnAction() { }
        public virtual void OnEnd() { }
    }

    class MonsterIdleState : MonsterState
    {
        float _searchTimer;

        public override void OnStart(Monster m)
        {
            base.OnStart(m);

            _m.PlayAnim(Anims.Idle);
            _searchTimer = _m._searchTime;
        }

        public override void OnAction()
        {
            _searchTimer += Time.deltaTime;
            if (_searchTimer > _m._searchTime)
            {
                _m._attackTarget = _m.FindMinDistAttackTarget();
                _searchTimer = 0f;
            }

            // ���� ����� �߰��ߴٸ� -> �߰� ���·� ����
            if (_m._attackTarget)
            {
                _m.SetState(new MonsterChaseState());
            }

            // ���� ��������Ʈ�� �ִٸ� -> �̵� ���·� ���� & ���� ��������Ʈ�� ������ ����
            if (_m._currentWayPointIndex < _m._waypoints.Count - 1)
            {
                Transform nextWaypoint = _m.FindNextWaypoint();
                MonsterMoveState state = new MonsterMoveState();
                state.SetDest(nextWaypoint.position);
                _m.SetState(state);
            }
        }
    }
    
    class MonsterMoveState : MonsterState
    {
        Vector3 _dest;
        float _searchTimer;

        public override void OnStart(Monster m)
        {
            base.OnStart(m);

            _m.PlayAnim(Anims.Walk);
            _m.Move(_m._waypoints[_m._currentWayPointIndex].position);
        }

        public void SetDest(Vector3 dest)
        {
            _dest = dest;
        }

        public override void OnAction()
        {
            _searchTimer += Time.deltaTime;
            if (_searchTimer > _m._searchTime)
            {
                _m._attackTarget = _m.FindMinDistAttackTarget();
                _searchTimer = 0f;
            }

            // ���� ����� �ִٸ� �Ѵ� ���·� ����
            if (_m._attackTarget)
            {
                _m.SetState(new MonsterChaseState());
                return;
            }
            // ���ݴ���� ���� ������ wayPoint�� �����ߴٸ� Idle���·� ����
            else if (!_m._attackTarget && _m._currentWayPointIndex < _m._waypoints.Count && _m._navMeshAgent.remainingDistance <= _m._navMeshAgent.stoppingDistance)
            {
                _m.SetState(new MonsterIdleState());
                return;
            }

            if (_dest != Vector3.zero)
                _m.Move(_dest);
        }
    }

    class MonsterChaseState : MonsterState
    {
        float _chaseDist = 0f;
        Vector3 _beforePos;
        public override void OnStart(Monster m)
        {
            base.OnStart(m);
            _beforePos = _m.transform.position;
            _m.PlayAnim(Anims.Walk);
        }

        public override void OnAction()
        {
            if (_m._navMeshAgent.velocity.magnitude < 1f)
                _m._attackTarget = _m.FindMinDistAttackTarget();

            // ���� ����� ������ٸ� Idle���·� ����
            if (!_m._attackTarget)
            {
                _m.SetState(new MonsterIdleState());
                return;
            }

            // attackRange �ۿ� �ְ� sightRange �ȿ� �ִٸ� �߰�
            float dist = _m.GetDistanceToAttackTarget();
            if (dist > _m._stat.AttackRange && dist < _m._stat.SightRange)
            {
                _m.Move(_m._attackTarget.transform.position);
                _chaseDist += Vector3.Distance(_m.transform.position, _beforePos);
                _beforePos = _m.transform.position;

                // �ִ� �߰� �Ÿ���ŭ �Ѿư��ٸ� 
                if (_chaseDist > _m._stat.MaxChaseDistance)
                {
                    _m.SetState(new MonsterReturningState());
                }
            }
            // attackRange �ȿ� �ִٸ� ���� ���·� ����
            else if (dist < _m._stat.AttackRange)
            {
                _m.SetState(new MonsterAttackState());
            }
        }
    }

    class MonsterAttackState : MonsterState
    {
        public override void OnStart(Monster m)
        {
            base.OnStart(m);
            _m._navMeshAgent.isStopped = true;
            _m._navMeshAgent.velocity = Vector3.zero;
            _m.PlayAnim(Anims.Idle);
        }

        public override void OnAction()
        {
            if (!_m._attackTarget)
            {
                _m.SetState(new MonsterIdleState());
                return;
            }

            // ���� ����� �þ� �ȿ� �ִٸ�
            if (_m.GetDistanceToAttackTarget() < _m._stat.SightRange)
            {
                // ���� ����� ���� ���� �ۿ� �ִٸ�
                if (_m.GetDistanceToAttackTarget() > _m._stat.AttackRange)
                {
                    _m.SetState(new MonsterChaseState());
                    return;
                }

                _m.FaceTarget();

                // ���� �غ� �ƴٸ� ����
                if (_m._isAttackReady)
                {
                    _m.PlayAnim(Anims.Attack);
                    _m.AttckTarget();
                    _m._isAttackReady = false;
                }
            }
            // ���� ����� �þ� �ۿ� �ִٸ�
            else
            {
                _m._attackTarget = null;
                _m.SetState(new MonsterReturningState());
            }
        }

        public override void OnEnd()
        {
            _m._navMeshAgent.isStopped = false;
        }
    }

    class MonsterReturningState : MonsterState
    {
        public override void OnStart(Monster m)
        {
            base.OnStart(m);
            Transform nextWaypoint = _m.FindNextWaypoint();
            if (nextWaypoint != null)
            {
                _m.Move(nextWaypoint.position);

                // ���� �̵� ������ ���� ���� ����� ���ٸ� �׳� Walking���·� �ٲ㼭 ���� ��� Ž���ϸ� �����ϱ�
                // ������ �ѹ� �� üũ
                Vector3 moveVec = (nextWaypoint.position - _m.transform.position).normalized;
                float moveDeg = Mathf.Atan2(moveVec.x, moveVec.z) * Mathf.Rad2Deg;
                if (moveDeg < 0f)
                    moveDeg += 360f;

                float minDeg = (_m._moveDeg - _m._degGap < 0f) ? _m._moveDeg - _m._degGap + 360f : _m._moveDeg - _m._degGap;
                float maxDeg = _m._moveDeg + _m._degGap;

                if (minDeg > maxDeg)
                {
                    if ((moveDeg > minDeg && moveDeg < 360f) || (moveDeg > 0f && moveDeg < maxDeg))
                    {
                        MonsterMoveState state = new MonsterMoveState();
                        state.SetDest(nextWaypoint.position);
                        _m.SetState(state);
                        return;
                    }
                }
                else
                {
                    if ((moveDeg > minDeg) && (moveDeg < maxDeg))
                    {
                        MonsterMoveState state = new MonsterMoveState();
                        state.SetDest(nextWaypoint.position);
                        _m.SetState(state);
                        return;
                    }
                }
            }

            _m.PlayAnim(Anims.Walk);
        }

        public override void OnAction()
        {
            if (_m._navMeshAgent.remainingDistance < _m._navMeshAgent.stoppingDistance + 0.1f)
            {
                _m.SetState(new MonsterIdleState());
            }
        }
    }

    class MonsterStiffingState : MonsterState
    {
        MonsterState _beforeState;
        float _stiffTime = 0.2f;

        public override void OnStart(Monster m)
        {
            base.OnStart(m);

            _m.StartCoroutine(_m.CoChangeColor(_stiffTime, Color.red));
            _m.StartCoroutine(CoChangeBeforeState(_stiffTime));
        }

        public void SetStiffTime(float stiffTime)
        {
            _stiffTime = stiffTime;
        }

        public void SetBeforeState(MonsterState beforeState)
        {
            _beforeState = beforeState;
        }

        IEnumerator CoChangeBeforeState(float stiffTime)
        {
            yield return new WaitForSeconds(stiffTime);
            _m.SetState(_beforeState);
        }
    }

    class MonsterDieState : MonsterState
    {
        public override void OnStart(Monster m)
        {
            base.OnStart(m);

            _m.PlayAnim(Anims.Dead);
            _m.GetComponent<CapsuleCollider>().enabled = false;
            _m._navMeshAgent.isStopped = true;
        }
    }
}