using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers _instance;
    public static Managers Instance { get { Init(); return _instance; } }

    public GameManager _game = new GameManager();

    public static GameManager Game { get { return Instance._game; } }

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
        }
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        _instance._game.UpdatePlayTime(Time.deltaTime);
    }
}
