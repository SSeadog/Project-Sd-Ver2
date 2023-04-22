using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    public override void Init()
    {
        LoadMap();
        LoadWorldObjects();

        CameraController cc = GameObject.FindObjectOfType<CameraController>();
        cc.Init();
    }

    public void LoadMap()
    {
        string path = "Prefabs/Maps/GameScene" + Managers.Game.StageNum + "Map";
        GameObject waypoints = Resources.Load<GameObject>(path);
        GameObject instance = Instantiate(waypoints);
        instance.name = "Map";
    }

    public void LoadWorldObjects()
    {
        Dictionary<string, Define.SettingInfo> settingInfo = Managers.Data.SettingInfo;
        List<string> keys = new List<string>(settingInfo.Keys);
        foreach (string key in keys)
        {
            GameObject original = Resources.Load<GameObject>(settingInfo[key].path);
            GameObject instance = Instantiate(original);
            instance.name = key;
            instance.transform.position = new Vector3(settingInfo[key].PosX, settingInfo[key].PosY, settingInfo[key].PosZ);
            instance.transform.eulerAngles = new Vector3(settingInfo[key].RotX, settingInfo[key].RotY, settingInfo[key].RotZ);

            if (key == Define.ObjectType.FriendlyTower.ToString() || key == Define.ObjectType.EnemyTower.ToString())
                instance.GetComponent<TowerBase>().Init();
            else if (key == Define.ObjectType.Player.ToString())
                Managers.Game.Player = instance;
        }
    }

    public override void Clear()
    {
        Destroy(Managers.Game.Player);
        Destroy(Managers.Game.FriendlyTower);
        Destroy(Managers.Game.EnemyTower);
    }
}
