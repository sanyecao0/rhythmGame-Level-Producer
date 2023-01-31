using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FllowPlayer : MonoBehaviour
{
    //��廬���벥�ſ���
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
            //ͨ�����̻��ֿ������������
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (transform.position.y <= 0)
                {
                    transform.position += new Vector3(0, 1 * scale);
                    songs.time -= 0.2f / 5.25f;
                    inputTimeString.text = Convert.ToDouble(songs.time).ToString("0.000");
                    Debug.Log("��ʱ��");

                }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {

                transform.position -= new Vector3(0, 1 * scale);
                songs.time += 0.2f / 5.25f;
                inputTimeString.text = Convert.ToDouble(songs.time).ToString("0.000");
                //����Ҫһ���޸Ĳ���ʱ��ķ���
                Debug.Log("��ʱ��");
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

