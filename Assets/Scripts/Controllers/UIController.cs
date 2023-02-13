using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    ESCMenuUI _escMenuUI;

    void Start()
    {
        _escMenuUI = transform.Find("ESCMenuUI").GetComponent<ESCMenuUI>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_escMenuUI.gameObject.activeSelf == false)
                _escMenuUI.ShowUI();
            else
                _escMenuUI.CloseUI();
        }
    }
}
