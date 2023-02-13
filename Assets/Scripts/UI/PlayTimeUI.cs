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
        string timeString = ZeroFill(Math.Round(time / 60).ToString()) + ":" + ZeroFill(Math.Round(time % 60).ToString());

        _timeText.text = timeString;
    }

    string ZeroFill(string s)
    {
        if (s.Length == 1)
        {
            return "0" + s;
        }
        else
        {
            return s;
        }
    }

    void Update()
    {
        SetTime(Managers.Game.playTime);
    }
}
