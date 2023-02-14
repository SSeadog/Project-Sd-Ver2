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

    private void Start()
    {
        // Todo
        // 스탯 표 json으로 만들어야함

        _maxHp = 100;
        _hp = _maxHp;
        _power = 5;
        _speed = 5f;
        _attackRange = 7f;
        _sightRange = 13f;
        _attackSpeed = 1f;
        _maxChaseDistance = 10f;
    }
}
