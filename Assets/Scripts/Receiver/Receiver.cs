using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

<<<<<<< HEAD
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
=======
public class Receiver : MonoBehaviour
{
    public SpriteRenderer sr;
    public double size;//�������Ŵ���
    public double alpha;//������ͼƬalphaֵ
    public List<NoteBase> NoteBase_List;
    public int state;//������״̬

    public Event SizeChange;//��С�����¼�
    public Event PositionChange;//λ�ø����¼�
    public Event AlphaChange;//͸�����޸��¼�
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// �������Ĵ�С�ı乫ʽ
    /// </summary>
    public void SizeChangeEvent(double BeginTime,double FinishTime,string formula,double TargetSize)
    {
   
    }
    /// <summary>
    /// ��������λ�øı䷽��
    /// </summary>
    public void PositionChangeEvent(double BeginTime, double FinishTime, string formula,double TargetX,double TargetY)
    {

    }
    /// <summary>
    /// ��������͸���ȸı䷽��
    /// </summary>
    public void AlphaChangeEvent(double BeginTime, double FinishTime, string formula,int TargetAlpha)
    {

    }
}
>>>>>>> 4b5ab1e819840b8603e3c581be9feedb23fdccea
