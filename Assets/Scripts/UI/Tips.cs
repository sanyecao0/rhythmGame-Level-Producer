using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class Tips : MonoBehaviour
{
    private string[] tips;
    int r;
    public GameObject tipMessage;
    private void Awake()
    {
        GetTips();
        //Debug.Log(tips.Length);
    }
    public void GetTips()
    {
        ReadTips("StreamingAssets/Tips.txt");
        r = Random.Range(0, tips.Length);
        (tipMessage).GetComponent<TMP_Text>().text = tips[r];
    }

    public void ReadTips(string Path)
    {
        
        FileStream fs = new FileStream($"{Application.dataPath}/{Path}", FileMode.Open, FileAccess.Read);
        StreamReader streamReader = new StreamReader(fs);
        tips = streamReader.ReadToEnd().Split(new string[] { "\r\n"}, StringSplitOptions.RemoveEmptyEntries);
        streamReader.Close();
        fs.Close();
    }
}
