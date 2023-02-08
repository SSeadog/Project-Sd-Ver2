using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStat : Stat
{
    [SerializeField] float _attackRange;
    [SerializeField] float _sightRange;
    [SerializeField] float _attackSpeed;

    public float AttackRange { get { return _attackRange; } set { _attackRange = value; } }
    public float SightRange { get { return _sightRange; } set { _sightRange = value; } }
    public float AttackSpeed { get { return _attackSpeed; } set { _attackSpeed = value; } }

    private void Start()
    {
        _maxHp = 300;
        _hp = _maxHp;
        _power = 10;
        _speed = 5f;
        _attackRange = 3f;
        _sightRange = 7f;
        _attackSpeed = 1f;
    }
}
