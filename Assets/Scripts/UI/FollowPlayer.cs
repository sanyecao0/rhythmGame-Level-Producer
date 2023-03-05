using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FollowPlayer : MonoBehaviour
{
    public GameObject NoteData;
    public GameObject EventData;
    //面板滑动与播放控制
    float scale = 0.35f;
    bool isPlay = false;
    public InputField inputTimeString;
    public AudioSource songs;
    void Update()
    {
        /*if (songs.time == 0)
        {
            NoteData.transform.position = new Vector3(0, 0, -7);
            EventData.transform.position = new Vector3(0, 0, -7);
       }*/
        if (isPlay)
        {
            float move = Time.deltaTime* -1.75f * (GameTime.Basic_BPM / 60f);
            NoteData.transform.Translate(0, move, 0);
            EventData.transform.Translate(0, move, 0);
        }
        else if(NoteData.transform.position.y<=0)
        {
           // Debug.Log("鼠标移动");
            //通过键盘滑轮控制物体的缩放
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (NoteData.transform.position.y < 0 && EventData.transform.position.y < 0)
                {
                    MoveUP();//减时间
                }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (NoteData.transform.position.y <= 0&&EventData.transform.position.y <= 0)
                {
                    MoveDown();//加时间
                }
            }
          }
        }
    private void MoveUP()
    {
        try
        {
            if (songs.time >= 0)
            {
                if((songs.time - scale / (1.75f * GameTime.Basic_BPM / 60f)) < 0)
                {
                    NoteData.transform.position = new Vector3(0, 0,-7);
                    EventData.transform.position = new Vector3(0, 0,-7);
                    songs.time = 0;
                    inputTimeString.text = Convert.ToDouble(songs.time).ToString("0.000");
                }
                else
                {
                    NoteData.transform.position += new Vector3(0, 1 * scale);
                    EventData.transform.position += new Vector3(0, 1 * scale);
                    songs.time = songs.time - scale / (1.75f * GameTime.Basic_BPM / 60f);
                    inputTimeString.text = Convert.ToDouble(songs.time).ToString("0.000");
                    Debug.Log("减时间");
                }
            }
        }
        catch {
            songs.time = 0;
            inputTimeString.text = Convert.ToDouble(songs.time).ToString("0.000");
        }
    }
    private void MoveDown()
    {
        try
        {
            if (songs.time <= GameTime.songsLength)
            {
                if (songs.time + scale / (1.75f * GameTime.Basic_BPM / 60f) > GameTime.songsLength)
                {
                    NoteData.transform.position -= new Vector3(0, 1 * scale);
                    EventData.transform.position -= new Vector3(0, 1 * scale);
                    songs.time = GameTime.songsLength;
                    inputTimeString.text = Convert.ToDouble(songs.time).ToString("0.000");
                }
                else
                {
                    NoteData.transform.position -= new Vector3(0, 1 * scale);
                    EventData.transform.position -= new Vector3(0, 1 * scale);
                    songs.time = songs.time + scale / (1.75f * GameTime.Basic_BPM / 60f);
                    inputTimeString.text = Convert.ToDouble(songs.time).ToString("0.000");
                    Debug.Log("加时间");
                }
            }
        }
        catch {
            songs.time = GameTime.songsLength;
            inputTimeString.text = Convert.ToDouble(songs.time).ToString("0.000");
        }
    }
    public void MusicPlay()
    {
        isPlay = true;
    }
    public void MusicPause()
    {
        isPlay = false;
    }
    public void InitAudioSource()
    {
        songs.Play();
        MusicPlay();
        songs.Pause();
        MusicPause();
    }
 }
