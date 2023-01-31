using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Receiver
{
    public double size;//接收器放大倍率
    public int alpha;//接收器图片alpha值
    public double Position_x;//初始化坐标x
    public double Position_y;//初始化坐标y
    public  List<NoteBase> Note=new List<NoteBase>(); //音符列表                                                     
   
    public Receiver (double size,int alpha,double x,double y)
    {
        this.size = size;
        this.alpha = alpha;
        this.Position_x = x;
        this.Position_y = y;
    }
}
public class ReciverEvent//接收器事件类（后续修改）
{
    public int type;
    public double BeginTime;
    public double FinishTime;
    public int FormulaType;
    public int TargetAlpha;
}
