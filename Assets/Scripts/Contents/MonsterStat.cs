using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStat : Stat
{
    [SerializeField] float _attackRange;
    [SerializeField] float _sightRange;
    [SerializeField] float _attackSpeed;
    [SerializeField] float _maxChaseDistance;

    public float AttackRange { get { return _attackRange; } set { _attackRange = value; } }
    public float SightRange { get { return _sightRange; } set { _sightRange = value; } }
    public float AttackSpeed { get { return _attackSpeed; } set { _attackSpeed = value; } }
    public float MaxChaseDistance { get { return _maxChaseDistance; } set { _maxChaseDistance = value; } }

    public void Init(Define.MonsterStat stat)
    {
        _maxHp = stat.MaxHp;
        _hp = _maxHp;
        _power = stat.Power;
        _speed = stat.Speed;
        _attackRange = stat.AttackRange;
        _sightRange = stat.SightRange;
        _attackSpeed = stat.AttackSpeed;
        _maxChaseDistance = stat.MaxChaseDistance;
    }

    private void Start()
    {
    }
}
