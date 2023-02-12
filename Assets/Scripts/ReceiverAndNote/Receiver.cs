using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Receiver
{
    public double size;//接收器放大倍率
    public int alpha;//接收器图片alpha值
    public float Position_x;//初始化坐标x
    public float Position_y;//初始化坐标y
    public  List<NoteBase> Note=new List<NoteBase>(); //音符列表
    public List<ReceiverEvent> Event = new List<ReceiverEvent>();//接收器事件列表
   
}
public class ReceiverEvent//0大小 1位置 2透明度
{
    public int EventType;
    public float start_Time;
    public float end_Time;
    public string formula;
    public string Target;
    public ReceiverEvent(int type, float BeginTime, float FinishTime, string formula, string Target)
    {
        this.EventType = type;
       this.start_Time = BeginTime;
        this.end_Time = FinishTime;
        this.formula = formula;
        this.Target = Target;
    }
}
public class ReceiverMes
{
    public Receiver Receiver=new Receiver();
    public ReceiverMes(double size, int alpha, float x, float y)
    {
        Receiver.size = size;
        Receiver.alpha = alpha;
        Receiver.Position_x = x;
        Receiver.Position_y = y;
    }
}

