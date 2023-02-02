using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NoteBase
{
    public float start_time;//ʱ���ʼʱ��
    public float Finish_time;//holdר������ʱ��
    public int type;//��������
    /*0->��ɫclick,1->��ɫclick��2-����click 3-��drag��4-��hold 6-��flick*/
    public float degree;//�Ƕ�
    public float speed;//Ĭ���ٶ�
    public bool fake;//�������

    public NoteBase(float stime, float ftime, int type, float angle, float speed, bool isfake)//holdר�ù��췽��
    {
        this.start_time = stime;
        this.Finish_time = ftime;
        this.type = type;
        this.degree = angle;
        this.speed = speed;
        this.fake = isfake;
    }
    public NoteBase(float stime, int t, float angle, float speed, bool isfake)//��ͨ�������췽��
    {
        this.start_time = stime;
        this.type = t;
        this.degree = angle;
        this.speed = speed;
        this.fake = isfake;
    }
}
