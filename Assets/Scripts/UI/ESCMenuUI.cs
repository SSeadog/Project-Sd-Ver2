using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ESCMenuUI : MonoBehaviour
{
    Button _restartButton;
    Button _toStagesButton;

    public void Init()
    {
        _restartButton = transform.Find("Panel/RestartButton").GetComponent<Button>();
        _toStagesButton = transform.Find("Panel/ToStagesButton").GetComponent<Button>();
    }

    public void ShowUI()
    {
        // 게임 멈추기
        Time.timeScale = 0f;

        Managers.Game.SetActiveCursor(true);

        gameObject.SetActive(true);
    }

    public void CloseUI(bool isMouseActive = false)
    {
        // 게임 재개
        Time.timeScale = 1f;

        Managers.Game.SetActiveCursor(isMouseActive);

        gameObject.SetActive(false);
    }

    void Start()
    {
        Init();
    }

    public void OnRestartButtonClicked()
    {
        CloseUI();
        Managers.Game.RestartStage();
    }

    public void OnToStagesButtonClicked()
    {
        CloseUI(true);
        Managers.Scene.LoadScene("StagesScene");
    }
}
