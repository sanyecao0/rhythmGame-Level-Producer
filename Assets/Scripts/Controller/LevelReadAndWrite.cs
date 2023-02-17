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
    public InputField BasicBPM;
    public InputField illustrator;

    public static LevelMessage lm;
    public  void SaveLevelMessage(){
        lm = new LevelMessage(TrackName.text, Artist.text, BPM.text, BasicBPM.text, TrackName.text);
        GameTime.Basic_BPM = float.Parse(lm.BasicBPM);
        LineRenders.SongCutNum = (GameTime.songsLength * GameTime.Basic_BPM / 60f);
    }

    public static void GetPath(string fatherPath)//��ȡ·��
    {
        FatherPath = fatherPath;
        SongsPath = FatherPath + "song.ogg";
        CoverPath = FatherPath + "cover.png";
        LevelMessagePath= FatherPath + "LevelMessage.txt";
        LevelPath = FatherPath + "0.txt";
        CheckLevel(fatherPath);
    }
    public static void CheckLevel(string fatherPath)//û�������ļ��Ļ����д���
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
        //��ȡ���淽��
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
        JsonMapper.ToJson(lm,levelm);
        StreamWriter writer2 = new StreamWriter(LevelMessagePath, false, System.Text.Encoding.UTF8);
        writer2.WriteLine(levelm);
        writer2.Flush();
        writer2.Close();
    }
    public static void ReadLevel()
    {
        StreamReader reader = new StreamReader(LevelPath);
        StreamReader LevMes = new StreamReader(LevelMessagePath);
        JsonReader Leveldata = new JsonReader(reader.ReadToEnd().ToString());
        JsonReader levelMesData = new JsonReader(LevMes.ReadToEnd().ToString());
        lm = JsonMapper.ToObject<LevelMessage>(levelMesData);
        Data.root=JsonMapper.ToObject<Root>(Leveldata);
        //Debug.Log(Data.root.NoteData.Count);
        reader.Close();
        LevMes.Close();
        //ʵ��������
    }
}
