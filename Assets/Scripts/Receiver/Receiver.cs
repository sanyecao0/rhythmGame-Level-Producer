using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

<<<<<<< HEAD
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
=======
public class Receiver : MonoBehaviour
{
    public SpriteRenderer sr;
    public double size;//接收器放大倍率
    public double alpha;//接收器图片alpha值
    public List<NoteBase> NoteBase_List;
    public int state;//接收器状态

    public Event SizeChange;//大小更改事件
    public Event PositionChange;//位置更改事件
    public Event AlphaChange;//透明度修改事件
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 接收器的大小改变公式
    /// </summary>
    public void SizeChangeEvent(double BeginTime,double FinishTime,string formula,double TargetSize)
    {
   
    }
    /// <summary>
    /// 接收器的位置改变方法
    /// </summary>
    public void PositionChangeEvent(double BeginTime, double FinishTime, string formula,double TargetX,double TargetY)
    {

    }
    /// <summary>
    /// 接收器的透明度改变方法
    /// </summary>
    public void AlphaChangeEvent(double BeginTime, double FinishTime, string formula,int TargetAlpha)
    {

    }
}
>>>>>>> 4b5ab1e819840b8603e3c581be9feedb23fdccea
