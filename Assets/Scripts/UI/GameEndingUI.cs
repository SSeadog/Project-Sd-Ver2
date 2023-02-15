using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        SetPanelTexts(_gameLosePanel);
        StartCoroutine(CoFadeUI(_gameLosePanel.GetComponent<Image>()));
    }

    public void ShowWinUI()
    {
        _gameWinPanel.gameObject.SetActive(true);
        SetPanelTexts(_gameWinPanel);
        StartCoroutine(CoFadeUI(_gameWinPanel.GetComponent<Image>()));
    }

    void SetPanelTexts(Transform panel)
    {
        TMP_Text gameTimeText = panel.transform.Find("TextPanel/GameTimeText").GetComponent<TMP_Text>();
        TMP_Text friendlyMonsterSpawnCountText = panel.transform.Find("TextPanel/FriendlyMonsterSpawnCountText").GetComponent<TMP_Text>();
        TMP_Text enemyMonsterSpawnCountText = panel.transform.Find("TextPanel/EnemyMonsterSpawnCountText").GetComponent<TMP_Text>();
        TMP_Text friendlyMonsterKilledCountText = panel.transform.Find("TextPanel/FriendlyMonsterKilledCountText").GetComponent<TMP_Text>();
        TMP_Text enemyMonsterKilledCountText = panel.transform.Find("TextPanel/EnemyMonsterKilledCountText").GetComponent<TMP_Text>();

        gameTimeText.text = "���� �ð� : " + Util.ConvertTime(Managers.Game.playTime);
        friendlyMonsterSpawnCountText.text = "��ȯ�� �Ʊ� ���� ��: " + Managers.Game.spawnedFriendlyMonsterCount;
        enemyMonsterSpawnCountText.text = "��ȯ�� �� ���� ��: " + Managers.Game.spawnedEnemyMonsterCount;
        friendlyMonsterKilledCountText.text = "���� �Ʊ� ���� ��: " + Managers.Game.killedFriendlyMonsterCount;
        enemyMonsterKilledCountText.text = "���� �� ���� ��: " + Managers.Game.killedEnemyMonsterCount;
    }

    IEnumerator CoFadeUI(Image img)
    {
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += 0.01f;
            img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }

    public void OnToStagesButton()
    {
        Managers.Scene.LoadScene("StagesScene");
    }
}
