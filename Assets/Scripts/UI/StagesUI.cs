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

    public void Init()
    {
        _gridPanel = transform.Find("Panel/GridPanel").gameObject;
        _stageIconOrigianl = Resources.Load<GameObject>("Prefabs/UI/SubItem/StageIcon");
        SetStageNums();
    }

    void SetStageNums()
    {
        _stageNums.Add(1);
        _stageNums.Add(2);
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
