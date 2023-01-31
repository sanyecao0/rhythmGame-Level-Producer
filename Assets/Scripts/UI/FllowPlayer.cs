using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
using UnityEngine.UI;
using System;

public class FllowPlayer : MonoBehaviour
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
                    transform.position += new Vector3(0, 1 * scale);
                    songs.time -= 0.2f / 5.25f;
                    inputTimeString.text = Convert.ToDouble(songs.time).ToString("0.000");
                    Debug.Log("减时间");

                }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {

                transform.position -= new Vector3(0, 1 * scale);
                songs.time += 0.2f / 5.25f;
                inputTimeString.text = Convert.ToDouble(songs.time).ToString("0.000");
                //还需要一个修改播放时间的方法
                Debug.Log("加时间");
            }
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
 }

=======

public class FllowPlayer : MonoBehaviour
{
    float scale = 0.2f;
    void Update()
    {
        //通过键盘滑轮控制物体的缩放
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (transform.position.y <= 0)
            {
                transform.position += new Vector3(0, 1 * scale);
                //还需要一个修改播放时间的方法
               // Debug.Log("加" + transform.position);
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {

            transform.position -= new Vector3(0, 1 * scale);
            //还需要一个修改播放时间的方法
            //Debug.Log("减"+transform.position);
        }

    }
}
>>>>>>> 4b5ab1e819840b8603e3c581be9feedb23fdccea
