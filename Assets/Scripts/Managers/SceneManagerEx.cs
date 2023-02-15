using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    public void LoadScene(string sceneName, int stageNum = -1)
    {
        Managers.Clear();

        Time.timeScale = 1f;
        Managers.Game.stageNum = stageNum;

        if (stageNum != -1)
        {
            Managers.Game.spawnInfo = Util.LoadJsonList<List<Define.spawnItem>>("Data/Stages/Spawn_" + stageNum);
        }

        SceneManager.LoadScene(sceneName);
    }

    public void Clear()
    {
        // _scene 할당 해제
        CurrentScene.Clear();
    }
}
