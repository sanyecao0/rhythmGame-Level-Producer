using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FollowPlayer : MonoBehaviour
{
    //面板滑动与播放控制
    float scale = 0.2f;
    bool isPlay = false;
    public InputField inputTimeString;
    public AudioSource songs;
    void Update()
    {
        if (isPlay)
        {
            float move = Time.deltaTime * -5.25f;
            transform.Translate(0, move, 0);
        }
        else
        {
            //通过键盘滑轮控制物体的缩放
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (transform.position.y <= 0)
                {
                    MoveUP();
                }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                MoveDown();
            }
          }
        }
    private void MoveUP()
    {
        transform.position += new Vector3(0, 1 * scale);
        songs.time = songs.time - 0.2f / 5.25f;
        inputTimeString.text = Convert.ToDouble(songs.time).ToString("0.000");
        Debug.Log("减时间");//如果进入编辑界面时没有点击播放按钮，那么该方法无效
        Debug.Log(songs.time);
    }
    private void MoveDown()
    {
        transform.position -= new Vector3(0, 1 * scale);
        songs.time = songs.time + 0.2f / 5.25f;
        inputTimeString.text = Convert.ToDouble(songs.time).ToString("0.000");
        Debug.Log("加时间");//如果进入编辑界面时没有点击播放按钮，那么该方法无效
        Debug.Log(songs.time);
    }
    public void MusicPlay()
    {
        isPlay = true;
    }
    public void MusicPause()
    {
        isPlay = false;
    }
 }
