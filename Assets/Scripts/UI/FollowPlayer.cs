using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FollowPlayer : MonoBehaviour
{
    public GameObject NoteData;
    public GameObject EventData;
    //��廬���벥�ſ���
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
           // Debug.Log("����ƶ�");
            //ͨ�����̻��ֿ������������
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (NoteData.transform.position.y < 0 && EventData.transform.position.y < 0)
                {
                    MoveUP();//��ʱ��
                }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (NoteData.transform.position.y <= 0&&EventData.transform.position.y <= 0)
                {
                    MoveDown();//��ʱ��
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
                    Debug.Log("��ʱ��");
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
                    Debug.Log("��ʱ��");
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
