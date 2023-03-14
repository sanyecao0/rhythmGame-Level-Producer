using UnityEngine;
using UnityEngine.UI;
using System.IO;
using LitJson;
using System.Text.RegularExpressions;
using System;

public class LevelReadAndWrite : MonoBehaviour
{
    public static string SongsPath;
    public static string CoverPath;
    public static string LevelMessagePath;
    public static string LevelPath;
    public static string FatherPath;

    public InputField TrackName;
    public InputField Artist;
    public InputField BPM;
    public InputField LevelDesign;
    public InputField BasicBPM;
    public InputField illustrator;
    public static bool SetLevelMessageBox = false;

    public  void SaveLevelMessage(){
        Data.levelMessage.Artist = Artist.text;
        Data.levelMessage.BasicBPM = BasicBPM.text;
        Data.levelMessage.LevelDesign = LevelDesign.text;
        Data.levelMessage.BPM = BPM.text;
        Data.levelMessage.TrackName = TrackName.text;
        Data.levelMessage.illustrator = illustrator.text;
        GameTime.Basic_BPM = float.Parse(Data.levelMessage.BasicBPM);
        LineRenders.SongCutNum = (GameTime.songsLength * GameTime.Basic_BPM / 60f);
        EventLines.SongCutNum = LineRenders.SongCutNum;
    }

    public static void GetPath(string fatherPath)//获取路径
    {
        FatherPath = fatherPath;
        SongsPath = FatherPath + "song.ogg";
        CoverPath = FatherPath + "cover.png";
        LevelMessagePath= FatherPath + "LevelMessage.json";
        LevelPath = FatherPath + "0.json";
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
        else
        {
            ReadLevel();
        }
        //读取谱面方法
    }
    public static void SaveLevel()
    {
        string data=JsonMapper.ToJson(Data.root);
        data= Regex.Unescape(data);
        StreamWriter writer = new StreamWriter(LevelPath,false,System.Text.Encoding.UTF8);
        writer.BaseStream.Seek(0, SeekOrigin.End);
        writer.WriteLine(data);
        writer.Flush();
        writer.Close();
        JsonWriter levelm = new JsonWriter();
        JsonMapper.ToJson(Data.levelMessage,levelm);
        StreamWriter writer2 = new StreamWriter(LevelMessagePath, false, System.Text.Encoding.UTF8);
        writer2.WriteLine(levelm);
        writer2.Flush();
        writer2.Close();
    }
    public static void ReadLevel()
    {
        StreamReader reader = new StreamReader(LevelPath);
        StreamReader LevMes = new StreamReader(LevelMessagePath);
        JsonReader levelMesData = new JsonReader(LevMes.ReadToEnd().ToString());
        JsonReader Leveldata = new JsonReader(reader.ReadToEnd().ToString());
        Data.root = JsonMapper.ToObject<Root>(Leveldata);
        Data.levelMessage = JsonMapper.ToObject<LevelMessage>(levelMesData);
        GameTime.Basic_BPM = float.Parse(Data.levelMessage.BasicBPM);
        SetLevelMessageBox = true;
        reader.Close();
        LevMes.Close();
        //实例化方法
    }
    public void QuitRLD()
    {
        Application.Quit();
    }
    public static void QuitRLDAndSave()
    {
        SaveLevel();
        Application.Quit();
    }
}
