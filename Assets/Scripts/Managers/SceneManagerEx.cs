using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    public void LoadScene(string sceneName)
    {
        Managers.Scene.LoadScene(sceneName);
    }

    public void Clear()
    {
        // _scene 할당 해제
        CurrentScene.Clear();
    }
}
