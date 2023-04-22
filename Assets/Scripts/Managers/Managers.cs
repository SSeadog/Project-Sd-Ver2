using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers _instance;
    public static Managers Instance { get { Init(); return _instance; } }

    DataManager _data = new DataManager();
    GameManager _game = new GameManager();
    SceneManagerEx _scene = new SceneManagerEx();

    public static DataManager Data { get { return Instance._data; } }
    public static GameManager Game { get { return Instance._game; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }

    static void Init()
    {
        if (_instance == null)
        {
            GameObject go = GameObject.Find("Managers");

            if (go == null)
            {
                go = new GameObject("Managers");
                _instance = go.AddComponent<Managers>();
            }

            Managers manager = go.GetComponent<Managers>();
            if (manager == null)
            {
                manager = go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);

            _instance = manager;
            _instance._game.Init();
            _instance._data.Init();
            Application.targetFrameRate = 60;
        }
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        if (_instance._game.StageNum != -1)
        {
            _instance._game.UpdatePlayTime(Time.deltaTime);
            _instance._game.UpdatePlayerRp(Time.deltaTime);
        }
    }

    public static void Clear()
    {
        _instance._data.Clear();
        _instance._game.Clear();
    }
}
