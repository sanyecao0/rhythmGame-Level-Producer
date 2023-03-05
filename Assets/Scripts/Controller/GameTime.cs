using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class GameTime : MonoBehaviour
{
    public static float Basic_BPM=120;//基础BPM
    public static float BPM;//BPM
    public static float songPosition;//歌曲位置
    public static float songPosInBeats;//记录当前所在节拍
    public static float secPerBeat;//单一拍时长
    public static float AudioOffset;//谱面偏移
    public static int BeatCutCount=4;//节拍细拆


    public InputField inputTimeString;
    public InputField inputBeatCut;
    public InputField inputBPM, inputAudioOffset;
    public AudioSource songs;
    public GameObject Mask;
    public GameObject NotePanel;
    public GameObject EventPanel;

    public static float songsLength;//给Line用的变量

    bool BPMChange = true;
    private void Awake()
    {
        StartCoroutine(LoadResource());
        if (Basic_BPM != 0 && BPMChange)
        {
            BPM = Basic_BPM;
            secPerBeat = 60f / BPM;//单一拍时长
            songPosition = songs.time;
        }
    }
    void Update()
    {
        if (Basic_BPM != 0&&BPMChange)
        {
            BPM = Basic_BPM;
            secPerBeat = 60f / BPM;//单一拍时长
            songPosition = songs.time;
        }
        if (songs.isPlaying)
        {
            songPosition = songs.time;
            songPosInBeats = songPosition / secPerBeat;//获得当前节拍位置
            inputTimeString.text = Convert.ToDouble(songs.time).ToString("0.000");//实时刷新歌曲位置
        }
    }
    IEnumerator LoadResource()
    {
        var uwr = UnityWebRequestMultimedia.GetAudioClip(LevelReadAndWrite.SongsPath, AudioType.OGGVORBIS);
            //Debug.Log("读取");
            yield return uwr.SendWebRequest();
            if (uwr.result !=UnityWebRequest.Result.ConnectionError)
            {
             AudioClip clip =DownloadHandlerAudioClip.GetContent(uwr);
            songs.clip = clip;
            songsLength = songs.clip.length;
            LineRenders.SongCutNum = songsLength /(Basic_BPM / 60f);
            EventLines.SongCutNum = songsLength / (Basic_BPM / 60f);
            /*while (true)
            {
                if (NoteDataManager.Ready && ReceiverManager.Ready && EventManager.Ready)
                    break;
                else
                    continue;
            }*/
            Mask.SetActive(false);//取消遮罩
        }
    }
    public void inputTime()//主动调整播放位置接口
    {
        if(float.Parse(inputTimeString.text)>=0&& float.Parse(inputTimeString.text) <= songs.clip.length)
        {
            songs.time = float.Parse(inputTimeString.text);
            NotePanel.transform.position = new Vector3(0,
                -1.75f * (songs.time * Basic_BPM) / 60f , -7);
            EventPanel.transform.position = new Vector3(0,
           -1.75f * (songs.time * Basic_BPM) / 60f , -7);
        }
        else
        {
            inputTimeString.text = Convert.ToDouble(songs.time).ToString("0.000");
        }
        //Debug.Log(songs.time);
    }
    public void inputBeatut()//主动调整节拍拆分接口
    {
        if(int.Parse(inputBeatCut.text)>=4&& int.Parse(inputBeatCut.text) <= 32)
        {
           BeatCutCount = int.Parse(inputBeatCut.text);
        }
        else
        {
            inputBeatCut.text =BeatCutCount.ToString();
        }
    }
    public void InputBasicBPM()//为基础BPM赋值的接口
    {

        try
        {
            Basic_BPM = float.Parse(inputBPM.text);
        }
        catch
        {
            Basic_BPM = 0;
        }

    }
    public void InputAudioOffset()//谱面偏移输入框赋值
    {
        try
        {
            AudioOffset = float.Parse(inputAudioOffset.text);
        }
        catch
        {
            AudioOffset = 0;
        }
    }
}
