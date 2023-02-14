using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : MonoBehaviour
{
    // Todo
    // Rp ¿¬µ¿

    Transform _hpBar;
    Transform _rpBar;

    public void Init()
    {
        _hpBar = transform.Find("Panel/HpBar/Foreground");
        _rpBar = transform.Find("Panel/RpBar/Foreground");
    }

    void Start()
    {
        Init();
    }

    public void SetHpBar(float percent)
    {
        _hpBar.localScale = new Vector3(percent, 1f, 1f);
    }

    public void SetRpBar(float percent)
    {
        _rpBar.localScale = new Vector3(percent, 1f, 1f);
    }

    void Update()
    {
        if (Managers.Game.player == null)
            return;

        PlayerStat player = Managers.Game.player.GetComponent<PlayerStat>();
        
        if (player.MaxHp == 0)
            return;

        float percent = (float)player.Hp / player.MaxHp;
        SetHpBar(percent);
    }
}
