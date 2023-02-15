using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerStat : Stat
{
    float _maxResourcePoint;
    float _resourcePoint;

    public float MaxResourcePoint { get { return _maxResourcePoint; } set { _maxResourcePoint = value; } }
    public float ResourcePoint { get { return _resourcePoint; } set { _resourcePoint = value; } }

    private void Start()
    {
        _maxHp = 300;
        _hp = _maxHp;
        _power = 0;
        _speed = 0;
        _maxResourcePoint = 100;
        _resourcePoint = _maxResourcePoint;
    }

    public bool UsePoint(float point)
    {
        if (point > _resourcePoint)
            return false;

        _resourcePoint -= point;

        return true;
    }
}
