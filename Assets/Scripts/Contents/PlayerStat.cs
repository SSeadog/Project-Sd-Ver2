using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    void Start()
    {
        _maxHp = 100;
        _hp = 100;
        _power = 10;
        _speed = 4f;
    }

    void Update()
    {
        
    }
}
