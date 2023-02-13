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
        // ���� ���߱�
        Time.timeScale = 0f;

        Managers.Game.SetActiveCursor();

        gameObject.SetActive(true);
    }

    public void CloseUI()
    {
        // ���� �簳
        Time.timeScale = 1f;

        Managers.Game.SetDeActiveCursor();

        gameObject.SetActive(false);
    }

    void Start()
    {
        Init();
    }

    public void OnRestartButtonClicked()
    {
        Managers.Game.RestartStage();
        CloseUI();
    }

    public void OnToStagesButtonClicked()
    {
        Managers.Scene.LoadScene("StagesScene");
    }
}
