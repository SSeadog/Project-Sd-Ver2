using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using static Define;

// FindMinDistAttackTarget()를 매 update마다 실행하는 건 부담이 있을 거 같은데
// 그래서 지금은 LookingForState, Walk상태에서만 찾도록 했는데
// 더 좋은 방법이 있을까?
// 1. FindMinDistAttackTarget()함수 비용 절감
// 2. 실행 주기에 텀 더주기

// 공격상태일 때 멈추는 기능 넣고 공격 상태 벗어날 때 다시 움직일 수 있도록 설정하는 부분 추가 필요

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
        // 현재 - 3 ~ 현재 + 2 사이 중 지금 위치에서 가장 가깝고 적절한 웨이포인트 찾기
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
                // 각도로 한번 더 체크
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

            // 공격 대상을 발견했다면 -> 추격 상태로 변경
            if (_m._attackTarget)
            {
                _m.SetState(new MonsterChaseState());
            }

            // 다음 웨이포인트가 있다면 -> 이동 상태로 변경 & 다음 웨이포인트로 목적지 설정
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

            // 공격 대상이 있다면 쫓는 상태로 변경
            if (_m._attackTarget)
            {
                _m.SetState(new MonsterChaseState());
                return;
            }
            // 공격대상이 없고 마지막 wayPoint에 도착했다면 Idle상태로 변경
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

            // 공격 대상이 사라졌다면 Idle상태로 변경
            if (!_m._attackTarget)
            {
                _m.SetState(new MonsterIdleState());
                return;
            }

            // attackRange 밖에 있고 sightRange 안에 있다면 추격
            float dist = _m.GetDistanceToAttackTarget();
            if (dist > _m._stat.AttackRange && dist < _m._stat.SightRange)
            {
                _m.Move(_m._attackTarget.transform.position);
                _chaseDist += Vector3.Distance(_m.transform.position, _beforePos);
                _beforePos = _m.transform.position;

                // 최대 추격 거리만큼 쫓아갔다면 
                if (_chaseDist > _m._stat.MaxChaseDistance)
                {
                    _m.SetState(new MonsterReturningState());
                }
            }
            // attackRange 안에 있다면 공격 상태로 변경
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

            // 공격 대상이 시야 안에 있다면
            if (_m.GetDistanceToAttackTarget() < _m._stat.SightRange)
            {
                // 공격 대상이 공격 범위 밖에 있다면
                if (_m.GetDistanceToAttackTarget() > _m._stat.AttackRange)
                {
                    _m.SetState(new MonsterChaseState());
                    return;
                }

                _m.FaceTarget();

                // 공격 준비가 됐다면 공격
                if (_m._isAttackReady)
                {
                    _m.PlayAnim(Anims.Attack);
                    _m.AttckTarget();
                    _m._isAttackReady = false;
                }
            }
            // 공격 대상이 시야 밖에 있다면
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

                // 다음 이동 방향이 원래 진행 방향과 같다면 그냥 Walking상태로 바꿔서 공격 대상 탐색하며 진행하기
                // 각도로 한번 더 체크
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