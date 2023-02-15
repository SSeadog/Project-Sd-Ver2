using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Util
{
    #region ReadJson
    public static T LoadJson<T>(string path)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        return JsonUtility.FromJson<T>(textAsset.text);
    }

    public static T LoadJsonList<T>(string path)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        return JsonConvert.DeserializeObject<T>(textAsset.text);
    }
    
    public static Dictionary<string, T> LoadJsonDict<T>(string path)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        return JsonConvert.DeserializeObject<Dictionary<string, T>>(textAsset.text);
    }
    #endregion

    #region TimeFormat
    public static string ConvertTime(float time)
    {
        return ZeroFill(Math.Floor(time / 60).ToString()) + ":" + ZeroFill(Math.Floor(time % 60).ToString());
    }
    #endregion

    public static string ZeroFill(string s)
    {
        if (s.Length == 1)
        {
            return "0" + s;
        }
        else
        {
            return s;
        }
    }
}
