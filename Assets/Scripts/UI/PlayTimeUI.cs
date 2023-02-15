using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayTimeUI : MonoBehaviour
{
    TMP_Text _timeText;

    void Start()
    {
        _timeText = transform.GetComponentInChildren<TMP_Text>();
    }

    void SetTime(float time)
    {
        _timeText.text = Util.ConvertTime(time);
    }

    void Update()
    {
        SetTime(Managers.Game.playTime);
    }
}
