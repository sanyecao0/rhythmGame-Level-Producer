using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Receiver
{
    public double size;//�������Ŵ���
    public int alpha;//������ͼƬalphaֵ
    public double Position_x;//��ʼ������x
    public double Position_y;//��ʼ������y
    public  List<NoteBase> Note=new List<NoteBase>(); //�����б�                                                     
   
    public Receiver (double size,int alpha,double x,double y)
    {
        this.size = size;
        this.alpha = alpha;
        this.Position_x = x;
        this.Position_y = y;
    }
}
public class ReciverEvent//�������¼��ࣨ�����޸ģ�
{
    public int type;
    public double BeginTime;
    public double FinishTime;
    public int FormulaType;
    public int TargetAlpha;
}
