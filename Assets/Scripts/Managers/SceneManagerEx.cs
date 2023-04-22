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
        Managers.Game.StageNum = stageNum;

        if (stageNum != -1)
        {
            Managers.Game.SpawnInfo = Util.LoadJsonList<List<Define.spawnItem>>("Data/Stages/Spawn_" + stageNum);
        }

        SceneManager.LoadScene(sceneName);
    }
}
