using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class LevelReadAndWrite : MonoBehaviour
{
    public static string SongsPath;
    public static string CoverPath;
    public static string LevelMessagePath;
    public static string LevelPath;
    public static string FatherPath;
    public static void GetPath(string fatherPath)//获取路径
    {
        FatherPath = fatherPath;
        SongsPath = FatherPath + "song.ogg";
        //Debug.Log(SongsPath);
        CoverPath = FatherPath + "cover.png";
        LevelMessagePath= FatherPath + "LevelMessage.txt";
        LevelPath = FatherPath + "0.txt";
        CheckLevel(fatherPath);
    }
    public static void CheckLevel(string fatherPath)//没有谱面文件的话自行创建
    {

        if (!File.Exists(LevelMessagePath))
        {
            File.Create(LevelMessagePath).Close();
        }
        if (!File.Exists(LevelPath))
        {
            File.Create(LevelPath).Close();
        }
        //读取谱面方法
    }
    public static void SaveLevel()
    {
        JsonWriter jw = new JsonWriter();
        JsonMapper.ToJson(Data.data,jw);
        StreamWriter writer = new StreamWriter(LevelPath,false,System.Text.Encoding.UTF8);
        writer.WriteLine(jw);
        writer.Flush();
        writer.Close();
    }
    public static void ReadLevel()
    {
        StreamReader reader = new StreamReader(LevelPath);
        JsonMapper.ToObject<Data>(reader.ToString());
        Debug.Log(Data.data);
    }
}
