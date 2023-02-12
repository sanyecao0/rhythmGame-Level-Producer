using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Receiver
{
    public double size;//�������Ŵ���
    public int alpha;//������ͼƬalphaֵ
    public float Position_x;//��ʼ������x
    public float Position_y;//��ʼ������y
    public  List<NoteBase> Note=new List<NoteBase>(); //�����б�
    public List<ReceiverEvent> Event = new List<ReceiverEvent>();//�������¼��б�
   
}
public class ReceiverEvent//0��С 1λ�� 2͸����
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

