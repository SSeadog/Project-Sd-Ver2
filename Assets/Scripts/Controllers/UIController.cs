using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public PlayerStatusUI _playerStatusUI;
    public PlayTimeUI _playTimeUI;
    public FriendlyMonsterPanelUI _friendlyMonsterPanelUI;
    public ESCMenuUI _escMenuUI;
    // MiniMapUI
    public GameEndingUI _gameEndingUI;

    void Start()
    {
        _playerStatusUI = transform.Find("PlayerStatusUI").GetComponent<PlayerStatusUI>();
        _playTimeUI = transform.Find("PlayTimeUI").GetComponent<PlayTimeUI>();
        _friendlyMonsterPanelUI = transform.Find("FriendlyMonsterPanelUI").GetComponent<FriendlyMonsterPanelUI>();
        _escMenuUI = transform.Find("ESCMenuUI").GetComponent<ESCMenuUI>();
        _gameEndingUI = transform.Find("GameEndingUI").GetComponent<GameEndingUI>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_escMenuUI.gameObject.activeSelf == false)
                _escMenuUI.ShowUI();
            else
                _escMenuUI.CloseUI();
        }
    }
}
