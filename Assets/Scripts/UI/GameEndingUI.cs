using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndingUI : MonoBehaviour
{
    Transform _gameLosePanel;
    Transform _gameWinPanel;

    public void Init()
    {
        _gameLosePanel = transform.Find("GameLosePanel");
        _gameWinPanel = transform.Find("GameWinPanel");
    }

    void Start()
    {
        Init();
    }

    public void ShowLoseUI()
    {
        _gameLosePanel.gameObject.SetActive(true);
    }

    public void ShowWinUI()
    {
        _gameWinPanel.gameObject.SetActive(true);
    }
}
