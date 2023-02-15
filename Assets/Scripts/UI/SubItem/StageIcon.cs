using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageIcon : MonoBehaviour
{
    int _stageNum;
    TMP_Text _stageNumText;

    public void Init()
    {
        _stageNumText = GetComponentInChildren<TMP_Text>();
    }

    void Start()
    {
        Init();
        _stageNumText.text = _stageNum.ToString();
    }

    public void SetStageNum(int stageNum)
    {
        _stageNum = stageNum;
    }

    public void OnStageIconClicked()
    {
        Managers.Scene.LoadScene($"GameScene{_stageNum}", _stageNum);
    }
}
