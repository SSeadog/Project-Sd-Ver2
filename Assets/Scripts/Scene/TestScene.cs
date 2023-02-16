using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : BaseScene
{
    public override void Init()
    {
        Dictionary<string, Define.SettingInfo> settingInfo = Managers.Data.settingInfo;
        List<string> keys = new List<string>(settingInfo.Keys);
        foreach (string key in keys)
        {
            GameObject original = Resources.Load<GameObject>(settingInfo[key].path);
            GameObject instance = Instantiate(original);
            instance.name = key;
            instance.transform.position = new Vector3(settingInfo[key].PosX, settingInfo[key].PosY, settingInfo[key].PosZ);
            instance.transform.eulerAngles = new Vector3(settingInfo[key].RotX, settingInfo[key].RotY, settingInfo[key].RotZ);

            if (key == Define.ObjectType.FriendlyTower.ToString())
            {
                instance.GetComponent<FriendlyTowerController>().Init(Define.ObjectType.FriendlyTower);
            }
            else if (key == Define.ObjectType.EnemyTower.ToString())
            {
                instance.GetComponent<EnemyTowerController>().Init(Define.ObjectType.EnemyTower);
            }
            else if (key == Define.ObjectType.Player.ToString())
            {
                Managers.Game.player = instance;
            }
        }

        CameraController cc = GameObject.FindObjectOfType<CameraController>();
        cc.Init();
    }

    public override void Clear()
    {
        Destroy(Managers.Game.player);
        Destroy(Managers.Game.friendlyTower);
        Destroy(Managers.Game.enemyTower);
    }
}
