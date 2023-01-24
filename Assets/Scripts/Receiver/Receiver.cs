using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
