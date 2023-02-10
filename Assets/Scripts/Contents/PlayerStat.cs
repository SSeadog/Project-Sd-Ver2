using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    void Start()
    {
        _maxHp = 100;
        _hp = 100;
        _power = 30;
        _speed = 10f;
    }

    void Update()
    {
        
    }
}
