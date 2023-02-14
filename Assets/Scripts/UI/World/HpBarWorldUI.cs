using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBarWorldUI : MonoBehaviour
{
    Transform _hpBar;

    Stat _stat;

    public void Init()
    {
        _hpBar = transform.Find("Panel/Foreground");
        _stat = transform.parent.GetComponent<Stat>();
    }

    void Start()
    {
        Init();
    }

    public void SetHp(float percent)
    {
        _hpBar.localScale = new Vector3(percent, 1f, 1f);
    }

    void Update()
    {
        if (_stat.MaxHp == 0f)
            return;

        float percent = (float)_stat.Hp / _stat.MaxHp;
        SetHp(percent);
    }
}
