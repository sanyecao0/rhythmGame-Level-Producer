using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD
public class NoteBase 
{
    public float start_time;//时间或开始时间
    public float Finish_time;//hold专属结束时间
    public int type;//音符种类
    /*0->红色click,1->白色click，2-》黑click 3-》drag，4-》hold 6-》flick*/
    public float degree;//角度
    public float speed;//默认速度
    public bool fake;//真假音符

    public NoteBase(float stime, float ftime,int t, float degree,float speed,bool isfake)//hold专用构造方法
    {
        this.start_time = stime;
        this.Finish_time = ftime;
        this.type = t;
        this.degree = degree;
        this.speed = speed;
        this.fake = isfake;
    }
    public NoteBase(float stime,  int t, float degree, float speed, bool isfake)//普通音符构造方法
    {
        this.start_time = stime;
        this.type = t;
        this.degree = degree;
        this.speed = speed;
        this.fake = isfake;
    }
=======
public abstract class NoteBase : MonoBehaviour
{
    public float time;
>>>>>>> 4b5ab1e819840b8603e3c581be9feedb23fdccea
}
