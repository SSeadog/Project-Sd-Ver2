using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class StagesUI : MonoBehaviour
{
    GameObject _gridPanel;
    GameObject _stageIconOrigianl;

    List<int> _stageNums = new List<int>();

    // ���ǿ��� UI��Ҵ� Init�� ������ Init�� public���� ������ Start���� �����ϵ��� �ϰ�
    // Text, �̹��� �� �����ؾ��ϴ� ��Ҵ� SetText(), SetImage() ������ �Լ��� �����ΰ� Instantiate�Ҷ� �����ϵ��� �ߴµ� �� �׷��� ������
    // Init()���� �޾Ƽ� ���� ���� ������? �ߴµ� Init�� �θ� ����� �����. �Ű����� �޴� ��縶�� �̸� �� �θ𿡼� ���Ǹ� �ص־��ϴµ� �װ� ���� ���� �� ����

    public void Init()
    {
        _gridPanel = transform.Find("Panel/GridPanel").gameObject;
        _stageIconOrigianl = Resources.Load<GameObject>("Prefabs/UI/SubItem/StageIcon");

        string path = Application.dataPath + "/Resources/Data/Stages";

        DirectoryInfo di = new DirectoryInfo(Path.Combine(Application.dataPath, "Resources/Data/Stages"));
        foreach (FileInfo file in di.GetFiles())
        {
            if (!file.FullName.Contains("Spawn"))
                continue;

            string[] splitedName = file.FullName.Split(".");
            if (splitedName[splitedName.Length - 1] != "meta")
            {
                int num = int.Parse(Regex.Replace(GetRealFileName(file.FullName), @"\D", ""));
                _stageNums.Add(num);
            }
        }
    }

    string GetRealFileName(string name)
    {
        string[] splitedName = name.Split("\\");
        return splitedName[splitedName.Length - 1];
    }

    void Start()
    {
        Init();

        foreach (int num in _stageNums)
        {
            GameObject instance = Instantiate(_stageIconOrigianl, _gridPanel.transform);
            instance.GetComponent<StageIcon>().SetStageNum(num);
        }
    }

}
