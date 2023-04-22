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
        Managers.Game.UIController = this;
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
