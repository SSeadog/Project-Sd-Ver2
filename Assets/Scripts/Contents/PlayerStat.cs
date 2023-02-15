using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    float _maxResourcePoint;
    float _resourcePoint;

    public float MaxResourcePoint { get { return _maxResourcePoint; } }
    public float ResourcePoint { get { return _resourcePoint; } set { _resourcePoint = value; } }

    void Start()
    {
        _maxHp = 100;
        _hp = _maxHp;
        _maxResourcePoint = 100f;
        _resourcePoint = 30f;
        _power = 30;
        _speed = 10f;
    }

    void Update()
    {
        
    }
}
