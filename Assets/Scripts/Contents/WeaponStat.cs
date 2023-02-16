using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStat : Stat
{
    Stat _baseStat;

    public void Init(Stat baseStat)
    {
        _baseStat = baseStat;

        _power = baseStat.Power;
    }

    public void Init()
    {
        if (_baseStat != null)
            _baseStat.Power = _power;
    }
}
